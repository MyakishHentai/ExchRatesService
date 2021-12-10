//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExchRates.Common.Middleware
//{
//    public class AuthorizationMiddleware
//    {
//        ILogger<GaiaAuthorizationMiddleware> _logger;
//        IGaiaService _gaiaService;

//        public GaiaAuthorizationMiddleware(IGaiaService gaiaService, ILogger<GaiaAuthorizationMiddleware> logger)
//        {
//            _gaiaService = gaiaService;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            context.Request.Query.TryGetValue("r", out StringValues rids);
//            context.Request.Query.TryGetValue("s", out StringValues sids);
//            context.Request.Query.TryGetValue("d", out StringValues deviceIds);

//            var rid = rids.FirstOrDefault();
//            var sid = sids.FirstOrDefault();
//            var deviceId = deviceIds.FirstOrDefault();

//            try
//            {
//                var isCorrectSession = await _gaiaService.CheckSessionAsync(rid, sid, deviceId);

//                if (!isCorrectSession)
//                {
//                    await SetErrorResponse(context, rid, "Invalid session", ResponseCodes.SessionNotFound);
//                    return;
//                }

//                var clientData = await _gaiaService.IsAutorizedClient(rid, sid);

//                if (!clientData.IsAuthorized)
//                {
//                    await SetErrorResponse(context, rid, "Client not authorized", ResponseCodes.NotAuthorized);
//                    return;
//                }

//                if (clientData.ClientCode <= 0)
//                {
//                    await SetErrorResponse(context, rid, "Unknown client", ResponseCodes.ClientNotFound);
//                    return;
//                }

//                var claim = new[] {
//                    new Claim(KnownCustomClaimTypes.ClientCode, clientData.Item2.ToString(), typeof(long).Name),
//                    new Claim(ClaimTypes.Role, KnownRoles.MobileApp)
//                };

//                var identity = new ClaimsIdentity(claim, KnownAutorizationSchemes.GaiaScheme);
//                context.User = new ClaimsPrincipal(identity);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//                await SetErrorResponse(context, rid, "Unknown autorization error", ResponseCodes.Internal);
//                return;
//            }

//            await next.Invoke(context);
//        }

//        public async Task SetErrorResponse(HttpContext context, string rid, string message, ResponseCodes responseCode)
//        {
//            var response = new AuthorizationResponse
//            {
//                Code = (int)responseCode,
//                Message = message,
//                RequestId = rid
//            };

//            context.Response.StatusCode = StatusCodes.Status200OK;
//            var serializeOptions = SerializeHelper.GetJsonSerializeOptions();
//            var json = JsonSerializer.Serialize(response, options: serializeOptions);
//            context.Response.ContentType = "application/json";
//            await context.Response.WriteAsync(json, new UTF8Encoding());
//        }
//    }
//}
