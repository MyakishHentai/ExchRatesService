using System;
using System.Text;
using ExchRates.Common.Caching.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ExchRates.Common.Caching
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _distCache;
        private readonly DistributedCacheEntryOptions _options;


        public DistributedCacheService(IDistributedCache cache, IConfiguration config)
        {
            _distCache = cache;
            var minutes = int.Parse(config["profiles: environmentVariables: CacheAddress"]);
            _options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(minutes))
                .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
        }

        public bool Get<T>(object key, out T data)
        {
            try
            {
                var dataEnc = _distCache.Get(key.ToString());
                if (dataEnc == null)
                {
                    data = default;
                    return false;
                }

                var dataDes = Encoding.UTF8.GetString(dataEnc);
                var dataType = JsonConvert.DeserializeObject<T>(dataDes);
                data = dataType;
                return true;
            }
            catch
            {
                data = default;
                return false;
            }
        }

        public void Remove(object key)
        {
            _distCache.Remove(key.ToString());
        }

        public void Set<T>(object key, T data, int liveTime)
        {
            var dataType = JsonConvert.SerializeObject(data);
            var dataEncoded = Encoding.UTF8.GetBytes(dataType);
            _distCache.Set(key.ToString(), dataEncoded, _options);
        }
    }
}