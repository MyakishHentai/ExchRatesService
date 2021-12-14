using ExchRates.Common.Repositories;
using ExchRates.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchRates.Common.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly ITokenService _tokenSvc;
        private readonly IConfiguration _config;


        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, ITokenService tokenSvc,
            IConfiguration config)
        {
            _logger = logger;
            _tokenSvc = tokenSvc;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var token = context.Session.GetString("Token");
                var path = context.Request.Path;
                if (path.HasValue && path.Value.StartsWith("/Authorization"))
                {
                    await next(context);
                    return;
                }
                if (string.IsNullOrEmpty(token))
                {
                    await SendError(context);
                    return;
                }
                context.Request.Headers.Add("Authorization", "Bearer " + token);
                if (_tokenSvc.IsTokenValid(_config["JwtKey"], _config["JwtIssuer"], token))
                {
                    await next(context);
                    return;
                }
                await SendError(context);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{DateTime.Now}]:Возникла ошибка при " +
                    $"выполнения действия {context.Request.Path}");
                throw;
            }
        }

        private async Task SendError(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var json = JsonSerializer.Serialize("Ошибка авторизации, отстутвует токен.");
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json, new UTF8Encoding());
        }
    }
}
