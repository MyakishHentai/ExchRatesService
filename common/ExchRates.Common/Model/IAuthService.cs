using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Model
{
    public interface IAuthService
    {
        Task<bool> CheckSessionAsync(string rid, string sid, string deviceId)
        {
            return Task.FromResult(true);
        }

        Task<(bool IsAuthorized, long ClientCode)> IsAutorizedClient(string rid, string sid)
        {
            return Task.FromResult((true, (long)123456789));
        }
    }
}
