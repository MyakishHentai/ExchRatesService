using ExchRates.Common.Model;

namespace ExchRates.Common.Repositories
{
    public interface IUserRepository
    {
        FakeUser GetUser(string userName, string password);
    }
}