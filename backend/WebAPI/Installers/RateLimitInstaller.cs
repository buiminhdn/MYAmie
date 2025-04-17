using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebAPI.Installers.Core;
using WebAPI.Options;

namespace WebAPI.Installers;

public class RateLimitInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var tokenPolicy = "tokenBucketLimiter";
        var myOptions = new RateLimitOptions();

        configuration.GetSection("RateLimitOptions").Bind(myOptions);

        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter(policyName: tokenPolicy, options =>
            {
                options.TokenLimit = myOptions.TokenLimit;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                options.QueueLimit = myOptions.QueueLimit;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(myOptions.ReplenishmentPeriod);
                options.TokensPerPeriod = myOptions.TokensPerPeriod;
            });
            // Customize rejection response
            options.OnRejected = (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return new ValueTask();
            };
        });
    }
}
