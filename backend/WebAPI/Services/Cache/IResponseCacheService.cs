﻿namespace WebAPI.Services.Cache;

public interface IResponseCacheService
{
    Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);

    Task<string> GetCacheResponseAsync(string cacheKey);

    Task RemoveCacheResponseAsync(string pattern);
}
