using System;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Text.Json;
using ExchRates.Common.Caching.Interfaces;

namespace ExchRates.Common.Caching
{
    public class FileCacheService : ICacheService
    {
        private readonly FileCache _binderCache;

        public FileCacheService(SerializationBinder binder, string cacheDir)
        {
            _binderCache = new FileCache(cacheDir, binder);
            _binderCache.CleanCache(cacheDir);
        }

        public bool Get<T>(object key, out T data)
        {
            try
            {
                var serialized = (string) _binderCache[key.ToString() ?? string.Empty];
                if (serialized != null)
                {
                    data = (T) JsonSerializer.Deserialize(serialized, typeof(T));
                    return data is { };
                }
                data = default;
                return false;
            }
            catch
            {
                data = default;
                return false;
            }
        }

        public void Remove(object key)
        {
            _binderCache.Remove(key.ToString() ?? string.Empty);
        }

        public void Set<T>(object key, T data, int liveTime)
        {
            _binderCache.AccessTimeout = TimeSpan.FromMinutes(liveTime);
            _binderCache[key.ToString() ?? string.Empty] = JsonSerializer.Serialize(data);
        }
    }
}