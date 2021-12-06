using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Caching
{
    public interface ICacheService
    {
        bool Get<T>(object key, out T data);
        
        void Set<T>(object key, T data, int liveTime);

        void Remove(object key);
    }
}
