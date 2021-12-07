using ExchRates.Common.Caching.Interfaces;
using System;

namespace ExchRates.Common.Caching
{
    public class FileCacheService : ICacheService
    {
        public bool Get<T>(object key, out T data)
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(object key, T data, int liveTime)
        {
            throw new NotImplementedException();
        }
    }
}
