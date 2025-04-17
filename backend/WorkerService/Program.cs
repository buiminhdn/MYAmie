using Amazon;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using WorkerService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddSecretsManager(
            region: RegionEndpoint.APSoutheast1,
            configurator: options =>
            {
                options.SecretFilter = entry => entry.Name == "myamie/secrets";
                options.KeyGenerator = (secret, key) =>
                {
                    if (!key.StartsWith(secret.Name + ":")) return null;
                    var newKey = key[(secret.Name.Length + 1)..].Replace("__", ":");
                    return newKey;
                };
                options.PollingInterval = null;
            });
    })
    .ConfigureServices((context, services) =>
    {
        var mailerSendOptions = context.Configuration.GetRequiredSection("MailerSend");
        var apiToken = mailerSendOptions["ApiToken"];
        var SenderName = mailerSendOptions["SenderName"];
        var SenderEmail = mailerSendOptions["SenderEmail"];

        services.AddMailerSend(options =>
        {
            options.ApiToken = apiToken;
            options.SenderName = SenderName;
            options.SenderEmail = SenderEmail;
        });

        services.AddDAL(context.Configuration);
        services.AddScoped<IEmailService, EmailService>(); // Add the EmailService to the DI container

        // Add hosted service
        services.AddHostedService<Worker>(); // Add the Worker class as a hosted service
    })
    .Build();

await host.RunAsync();