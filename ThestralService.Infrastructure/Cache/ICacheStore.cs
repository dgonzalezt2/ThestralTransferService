namespace ThestralService.Infrastructure.Cache;

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

internal interface ICacheStore
{
    Task<T?> GetAsync<T>(string key, CancellationToken ctx) where T : class;

    Task SaveAsync<T>(string key, T value, CancellationToken ctx, TimeSpan? expiration = null) where T : class;
}
