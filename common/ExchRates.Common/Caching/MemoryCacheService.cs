using ExchRates.Common.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ExchRates.Common.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache cache)
        {
            _memoryCache = cache;
        }

        public bool Get<T>(object key, out T data)
        {
            if (_memoryCache.TryGetValue(key, out data))
            {
                return true;
            }
            data = default(T);
            return false;
        }

        public void Remove(object key)
        {
            _memoryCache.Remove(key);
        }

        public void Set<T>(object key, T data, int liveTime)
        {
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(liveTime));
        }
    }
}
