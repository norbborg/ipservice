using Microsoft.Extensions.Caching.Memory;

namespace IpService;

public class MemoryCacheStore : ICacheStore
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheStore(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public void Add<T>(string key,T item, TimeSpan expiration)
    {
        _memoryCache.Set(key, item, expiration);
    }

    public T? Get<T>(string key) where T : class
    {
        return _memoryCache.TryGetValue(key, out T? value) ? value : null;
    }
}