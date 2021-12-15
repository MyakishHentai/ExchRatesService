namespace ExchRates.Common.Caching.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        ///     Получение объекта из кэша.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Десериализованный объект из кэша.</param>
        /// <returns>Существует ли в кэше данный тип по ключу <paramref name="key"/></returns>
        bool Get<T>(object key, out T data);

        /// <summary>
        ///     Установка объекта в кэш на заданное время.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="key">Ключ.</param>
        /// <param name="data">Объект.</param>
        /// <param name="liveTime">Время хранения объекта в кэше (в минутах).</param>
        void Set<T>(object key, T data, int liveTime);

        /// <summary>
        ///     Удалить объект из кэша (очистить).
        /// </summary>
        /// <param name="key">Ключ.</param>
        void Remove(object key);
    }
}