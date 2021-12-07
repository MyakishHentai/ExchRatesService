namespace ExchRates.Common.Caching.Interfaces
{
    public interface ICacheService
    {
        bool Get<T>(object key, out T data);

        void Set<T>(object key, T data, int liveTime);

        void Remove(object key);
    }
}
