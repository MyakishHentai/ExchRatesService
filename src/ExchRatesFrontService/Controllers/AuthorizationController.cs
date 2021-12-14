using ExchRates.Common.Repositories;
using ExchRates.Common.Services;
using ExchRatesFrontService.Config;
using ExchRatesFrontService.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ExchRatesFrontService.Controllers
{
    /// <summary>
    ///     Контроллер, отвечающий за авторизацию пользователя в системе.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly ITokenService _tokenSvc;
        private readonly IUserRepository _userRep;
        private string _token = null;

        public AuthorizationController(ILogger<AuthorizationController> logger,
                               IConfiguration config, 
                               ITokenService tokenSvc, 
                               IUserRepository userRep)
        {
            _logger = logger;
            _config = config;
            _tokenSvc = tokenSvc;
            _userRep = userRep;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromQuery] LoginRequest logRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(logRequest.UserName) || string.IsNullOrEmpty(logRequest.Password))
                    return Unauthorized("Заполните обязательные поля для авторизации.");
                
                var validUser = _userRep.GetUser(logRequest.UserName, logRequest.Password);

                if (validUser != null)
                {
                    var asd = _config["JwtKey"];
                    _token = _tokenSvc.BuildToken(_config["JwtKey"], _config["JwtIssuer"], validUser);
                    if (_token != null)
                    {
                        HttpContext.Session.SetString("Token", _token);
                        return Ok("Вы успешно авторизованы.");
                    }
                    return Unauthorized("Ошибка генерации токена.");
                }
                return Unauthorized("Пользователь не найден.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Сервис временно недоступен");
            }
        }
    }
}
