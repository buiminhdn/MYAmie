using DAL;
using MailerSend.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Models.Core;
using Models.Emails;
using Utility;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly MailerSendService _mailerSend;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, MailerSendService mailerSend)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _mailerSend = mailerSend;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            try
            {
                await SendMails(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending emails.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromDays(7), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // Expected on shutdown
            }
        }
    }

    private async Task SendMails(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Worker>>();

        // Get all pending marketing emails that haven't been sent
        var marketingEmails = await dbContext.Emails
            .Where(e => e.Type == EmailType.MARKETING && e.Status == EmailStatus.PENDING)
            .ToListAsync(cancellationToken);

        if (marketingEmails.Count == 0)
        {
            logger.LogInformation("No marketing emails to send.");
            return;
        }

        //Get all user emails
        var userEmails = await dbContext.Accounts
            .Where(a => a.IsEmailVerified && a.Status == AccountStatus.ACTIVATED && a.Role != Role.ADMIN)
            .Select(a => a.Email)
            .ToListAsync(cancellationToken);

        if (userEmails.Count == 0)
        {
            logger.LogWarning("No verified users found to send emails to.");
            return;
        }

        foreach (var email in marketingEmails)
        {
            var throttler = new SemaphoreSlim(10); // Limit concurrency to 10
            var tasks = userEmails.Select(userEmail => SendToUserAsync(userEmail, email, throttler, logger, cancellationToken));

            await Task.WhenAll(tasks);

            email.Status = EmailStatus.SENT;
            email.UpdatedDate = DateTimeUtils.TimeInEpoch();
            dbContext.Emails.Update(email);
            logger.LogInformation("Marketing email ID {Id} marked as sent.", email.Id);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SendToUserAsync(string userEmail, Email email, SemaphoreSlim throttler, ILogger logger, CancellationToken cancellationToken)
    {
        await throttler.WaitAsync(cancellationToken);
        try
        {
            var to = new List<Recipient>
            {
                new()
                {
                    Email = userEmail,
                    Substitutions = new Dictionary<string, string>
                    {
                        { "email", userEmail }
                    }
                }
            };

            await _mailerSend.SendMailAsync(
                to: to,
                subject: email.Subject,
                html: email.Body,
                cancellationToken: cancellationToken
            );

            logger.LogInformation("Email sent to {Email} for subject: {Subject}", userEmail, email.Subject);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send email to {Email} with subject {Subject}", userEmail, email.Subject);
        }
        finally
        {
            throttler.Release();
        }
    }
}
