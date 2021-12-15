using ExchRates.Common.Model;

namespace ExchRates.Common.Services
{
    public interface ITokenService
    {
        /// <summary>
        ///     Генерация токена JWT.
        /// </summary>
        /// <param name="key">Ключ JWT.</param>
        /// <param name="issuer">Издатель.</param>
        /// <param name="user">Пользователь системы.</param>
        /// <returns>Токен.</returns>
        string BuildToken(string key, string issuer, FakeUser user);

        /// <summary>
        ///     Проверка токена на соответствие и действительность.
        /// </summary>
        /// <param name="key">Ключ JWT.</param>
        /// <param name="issuer">Издатель.</param>
        /// <param name="token">Токен.</param>
        /// <returns>Результат проверки.</returns>
        bool IsTokenValid(string key, string issuer, string token);
    }
}