using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace WebAPI.Services.Cache;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ILogger<ResponseCacheService> _logger;

    public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, ILogger<ResponseCacheService> logger)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
        _logger = logger;
    }

    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
        try
        {
            return await _distributedCache.GetStringAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cache for key: {CacheKey}", cacheKey);
            return null;
        }
    }

    public async Task RemoveCacheResponseAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentNullException(nameof(pattern), "Value cannot be null or whitespace");

        try
        {
            var keys = GetKeysAsync(pattern + "*");
            await Parallel.ForEachAsync(keys, async (key, _) => await TryRemoveCacheAsync(key));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cache entries for pattern: {Pattern}", pattern);
        }
    }

    private async Task TryRemoveCacheAsync(string key)
    {
        try
        {
            await _distributedCache.RemoveAsync(key);
            _logger.LogInformation("Cache removed for key: {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove cache for key: {CacheKey}", key);
        }
    }

    private async IAsyncEnumerable<string> GetKeysAsync(string pattern)
    {
        foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);
            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                yield return key.ToString();
            }
        }
    }

    public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
    {
        if (response == null)
            return;

        try
        {
            var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });

            _logger.LogInformation("Cache set for key: {CacheKey}", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache for key: {CacheKey}", cacheKey);
        }
    }
}
