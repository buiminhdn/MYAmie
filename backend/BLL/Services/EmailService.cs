using BLL.Interfaces;
using Common.DTOs.EmailDtos;
using Common.Pagination;
using Common.Responses;
using Common.ViewModels.AuthVMs;
using Common.ViewModels.EmailVMs;
using DAL.Repository.Core;
using MailerSend.AspNetCore;
using Microsoft.Extensions.Logging;
using Models.Emails;
using System.Linq.Expressions;
using System.Text.Json;
using Utility;
using Utility.Constants;

namespace BLL.Services;
public class EmailService(IUnitOfWork unitOfWork, ILogger<EmailService> logger, MailerSendService mailerSend) : IEmailService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<EmailService> _logger = logger;
    private readonly MailerSendService _mailerSend = mailerSend;

    public async Task<ApiResponse> SendVerifyEmail(string email, string verificationCode, VerificationTypeParam type)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetAsync(a => a.Email == email);
            if (account == null)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.AccountNotFound);

            if (type == VerificationTypeParam.AccountConfirmation && account.IsEmailVerified)
                return ApiResponse<AuthAccountVM>.Failure(ResponseMessages.EmailVerified);

            string templateFile = type == VerificationTypeParam.AccountConfirmation
                          ? "BLL.Resources.EmailVerification.json"
                          : "BLL.Resources.PasswordReset.json";

            string json = ResourceHelper.ReadEmbeddedResource(templateFile);

            var template = JsonSerializer.Deserialize<EmailTemplate>(json);

            if (template == null)
            {
                _logger.LogError("An error occurred while reading email template.");
                return ApiResponse.Failure(ResponseMessages.EmailNotSent);
            }

            var to = new List<Recipient>
            {
                new()
                {
                    Email = email,
                    Name = account.FirstName,
                    Substitutions = new Dictionary<string, string>
                    {
                        { "name", account.FirstName },
                        { "code", verificationCode }
                    }
                }
            };

            // Send email
            await _mailerSend.SendMailAsync(
                to: to,
                subject: template.Subject,
                html: template.Body,
                text: $"Your verification code is: {verificationCode}" // Fallback
            );

            _logger.LogInformation("Verification email sent to {Email}.", email);
            return ApiResponse.Success(ResponseMessages.EmailSent);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification email to {Email}", email);
            throw new InvalidOperationException(ResponseMessages.EmailNotSent, ex);
        }
    }

    public async Task<ApiResponse> SetEmailVerified(string email)
    {
        try
        {
            var account = await _unitOfWork.Accounts.GetAsync(a => a.Email == email);

            if (account == null)
                return ApiResponse.Failure(ResponseMessages.AccountNotFound);

            if (account.IsEmailVerified)
                return ApiResponse.Failure(ResponseMessages.EmailVerified);

            account.IsEmailVerified = true;

            await _unitOfWork.Accounts.UpdateAsync(account);
            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.EmailVerified)
               : ApiResponse.Failure(ResponseMessages.EmailNotVerified);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying email for {Email}.", email);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> AddMarketingEmail(string subject, string body)
    {
        try
        {
            // Create the marketing email object
            var marketingEmail = new Email
            {
                SenderEmail = "marketing@mymail.com", // Replace with your marketing email
                ReceiverEmail = "", // Empty for now
                Subject = subject,
                Body = body,
                Status = EmailStatus.PENDING,
                Type = EmailType.MARKETING,
                CreatedBy = 1,
            };

            // Store the email in the database and save changes
            await _unitOfWork.Emails.AddAsync(marketingEmail);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                _logger.LogInformation("Marketing email '{Subject}' created and stored successfully.", subject);
                return ApiResponse.Success(ResponseMessages.MarketingEmailSentSuccess);
            }

            return ApiResponse.Failure(ResponseMessages.MarketingEmailSentFailure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating and storing marketing email '{Subject}'.", subject);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse<PagedEmailMarketingVM>> GetEmailMarketings(EmailMarketingParams param)
    {
        try
        {
            Expression<Func<Email, bool>> filter = email =>
                email.Type == EmailType.MARKETING;
            var emails = await _unitOfWork.Emails.GetAllAsync(filter);

            var emailVMs = emails.Select(email => new EmailMarketingVM
            {
                Id = email.Id,
                Subject = email.Subject,
                Body = email.Body,
                Status = email.Status.ToString() // Manually map the enum to string
            }).ToList();

            var pagedEmails = PagedList<EmailMarketingVM>.ToPagedList(emailVMs, param.PageNumber, param.PageSize);

            var pagedEmailsVM = new PagedEmailMarketingVM
            {
                Emails = pagedEmails,
                Pagination = pagedEmails.PaginationData
            };

            return ApiResponse<PagedEmailMarketingVM>.Success(pagedEmailsVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving emails by admin with params: {@Param}", param);
            return ApiResponse<PagedEmailMarketingVM>.Failure(ResponseMessages.UnexpectedError);
        }
    }

    public async Task<ApiResponse> DeleteMarketingEmail(int id)
    {
        try
        {
            var email = await _unitOfWork.Emails.GetByIdAsync(id);

            if (email == null)
                return ApiResponse.Failure(ResponseMessages.EmailNotFound);

            await _unitOfWork.Emails.DeleteAsync(email);

            return await _unitOfWork.SaveAsync() > 0
               ? ApiResponse.Success(ResponseMessages.EmailDeleted)
               : ApiResponse.Failure(ResponseMessages.EmailNotDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting place with ID: {Id}", id);
            return ApiResponse.Failure(ResponseMessages.UnexpectedError);
        }
    }
}
