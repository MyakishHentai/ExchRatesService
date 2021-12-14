using ExchRates.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Repositories
{
    public interface IUserRepository
    {
        FakeUser GetUser(string userName, string password);
    }
}
