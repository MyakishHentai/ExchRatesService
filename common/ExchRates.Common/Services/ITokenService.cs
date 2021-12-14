using ExchRates.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, FakeUser user);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
