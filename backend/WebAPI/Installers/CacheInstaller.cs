
using StackExchange.Redis;
using WebAPI.Installers.Core;
using WebAPI.Options;
using WebAPI.Services.Cache;

namespace WebAPI.Installers;

public class CacheInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = new RedisOption();

        configuration.GetSection("RedisOption").Bind(redisConfiguration);

        services.AddSingleton(redisConfiguration);

        if (!redisConfiguration.Enabled)
        {
            return;
        }

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));

        services.AddStackExchangeRedisCache(options => options.Configuration = redisConfiguration.ConnectionString);

        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}
