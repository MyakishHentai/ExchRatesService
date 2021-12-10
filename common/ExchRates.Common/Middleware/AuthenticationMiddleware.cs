//using ExchRates.Common.Repositories;
//using ExchRates.Common.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace ExchRates.Common.Middleware
//{
//    public class AuthenticationMiddleware : IMiddleware
//    {
//        private readonly ILogger<AuthenticationMiddleware> _logger;
//        private readonly IConfiguration _config;

//        private readonly IUserRepository _userRep;
//        private readonly ITokenService _tokenSvc;
//        private string _token = string.Empty;
//        public AuthenticationMiddleware(ILogger<AuthenticationMiddleware> logger, ITokenService tokenSvc,
//            IUserRepository userRep, IConfiguration configuration)
//        {
//            _logger = logger;
//            _tokenSvc = tokenSvc;
//            _userRep = userRep;
//            _config = configuration;
//        }

//        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            try
//            {
//                var token = context.Session.GetString("Token");
//                if (!string.IsNullOrEmpty(token))
//                {
//                    context.Request.Headers.Add("Authorization", "Bearer " + token);
//                }
//                //await AuthWorkAsync(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, @$"[{DateTime.Now}]:Возникла ошибка при 
//                                        выполнения действия {context.Request.Path}");
//                throw;
//            }
//            finally
//            {
//                await next(context);
//            }
//        }

//        public async Task AuthWorkAsync(HttpContext context)
//        {
//            try
//            {
//                //Reading the AuthHeader which is signed with JWT
//                string authHeader = context.Request.Headers["Authorization"];
//                await Task.Run(() =>
//                {
//                    if (authHeader != null)
//                    {
//                        //Reading the JWT middle part           
//                        int startPoint = authHeader.IndexOf(".") + 1;
//                        int endPoint = authHeader.LastIndexOf(".");

//                        var tokenString = authHeader[startPoint..endPoint].Split(".");
//                        var token = tokenString[0].ToString() + "==";

//                        var credentialString = Encoding.UTF8
//                            .GetString(Convert.FromBase64String(token));

//                        // Splitting the data from Jwt
//                        var credentials = credentialString.Split(new char[] { ':', ',' });

//                        // Trim this Username and UserRole.
//                        var userRule = credentials[5].Replace("\"", "");
//                        var userName = credentials[3].Replace("\"", "");

//                        // Identity Principal
//                        var claims = new[]
//                        {
//                            new Claim("name", userName),
//                            new Claim(ClaimTypes.Role, userRule),
//                        };
//                        var identity = new ClaimsIdentity(claims, "basic");
//                        context.User = new ClaimsPrincipal(identity);
//                    }
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Не удалось авторизоваться.");
//            }
//        }

//        public bool Login(
//            [FromQuery] BaseRequest baseRequest,
//            [FromForm] LoginRequest request)
//        {
//            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
//                return ErrorAuthentication;

//            var validUser = _userRep.GetUser(request.UserName, request.Password);

//            if (validUser != null)
//            {
//                _token = _tokenSvc.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
//                if (_token != null)
//                {
//                    HttpContext.Session.SetString("Token", _token);
//                    return SuccessAuthentication;
//                }
//                return ErrorAuthentication;
//            }
//            return ErrorAuthentication;
//        }

//        private bool ErrorAuthentication
//        {
//            get
//            {
//                _logger.LogError("Авторизация/аутентификация не пройдена.");
//                HttpContext.Response.StatusCode = 403;
//                return false;
//            }
//        }

//        private bool SuccessAuthentication
//        {
//            get
//            {
//                _logger.LogError("Авторизация/аутентификация успешно пройдена.");
//                HttpContext.Response.StatusCode = 200;
//                return true;
//            }
//        }

//        private bool InSession()
//        {
//            string token = HttpContext.Session.GetString("Token");
//            if (token == null)
//            {
//                return ErrorAuthentication;
//            }
//            if (!_tokenSvc.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
//            {
//                return ErrorAuthentication;
//            }
//            return SuccessAuthentication;
//        }
//    }
//}
