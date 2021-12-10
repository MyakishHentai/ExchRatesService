//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExchRates.Common.Model
//{
//    public class AuthService : IAuthService
//    {
//        private readonly ILogger<AuthService> _logger;

//        public AuthService(ILogger<AuthService> logger)
//        {
//            _logger = logger;
//        }


//        public async Task<bool> CheckSessionAsync(string rid, string sid, string deviceId)
//        {
//            CheckSessionReply reply;
//            try
//            {
//                CheckSessionRequest checkSessionRequest = new CheckSessionRequest
//                {
//                    Rid = rid,
//                    Sid = sid,
//                    DeviceId = deviceId
//                };

//                var deadline = DateTime.UtcNow.AddMilliseconds(_gaiaServiceOptions.TimeoutInMs);
//                reply = await _client.CheckSessionAsync(checkSessionRequest, deadline: deadline);
//                if (reply.Result.ResCode == ResCode.Ok)
//                    return true;
//            }
//            catch (RpcException e)
//            {
//                _logger.LogError(e, $"Status: Code = {e.Status.StatusCode}[{(int)e.Status.StatusCode}], Detail = {e.Status.Detail}");
//            }

//            return false;
//        }

//        public async Task<(bool IsAuthorized, long ClientCode)> IsAutorizedClient(string rid, string sid)
//        {
//            try
//            {
//                GetSessionRequest getSessionRequest = new GetSessionRequest
//                {
//                    Rid = rid,
//                    Sid = sid
//                };
//                var deadline = DateTime.UtcNow.AddMilliseconds(_gaiaServiceOptions.TimeoutInMs);
//                var reply = await _client.GetSessionAsync(getSessionRequest, deadline: deadline);
//                if (reply.Result.ResCode == ResCode.Ok && reply.SessionData.Auth)
//                    return (true, reply.SessionData.User.ClientCode);
//            }
//            catch (RpcException e)
//            {
//                _logger.LogError(e, $"Status: Code = {e.Status.StatusCode}[{(int)e.Status.StatusCode}], Detail = {e.Status.Detail}");
//            }

//            return (false, 0);
//        }
//    }
//}
