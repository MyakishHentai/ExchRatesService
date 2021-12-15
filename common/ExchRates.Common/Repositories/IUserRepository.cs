using ExchRates.Common.Model;

namespace ExchRates.Common.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        ///     Полуение пользователя из базы.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Пользователь.</returns>
        FakeUser GetUser(string userName, string password);
    }
}