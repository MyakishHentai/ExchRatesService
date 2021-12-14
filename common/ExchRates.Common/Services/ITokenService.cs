using ExchRates.Common.Model;

namespace ExchRates.Common.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, FakeUser user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}