using System;
using ExchRates.Common.Repositories;
using ExchRates.Common.Services;
using ExchRatesFrontService.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        private string _token;

        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            IConfiguration config,
            ITokenService tokenSvc,
            IUserRepository userRep)
        {
            _logger = logger;
            _config = config;
            _tokenSvc = tokenSvc;
            _userRep = userRep;
        }


        /// <summary>
        ///     Аутентификация путем установления JWT в сессии.
        /// </summary>
        /// <param name="logRequest">Минимальные данные для авторизации.</param>
        /// <returns>Код ответа на запрос.</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromQuery] LoginRequest logRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(logRequest.UserName) || string.IsNullOrEmpty(logRequest.Password))
                    return Unauthorized("Заполните обязательные поля для авторизации.");

                var validUser = _userRep.GetUser(logRequest.UserName, logRequest.Password);

                if (validUser == null) return Unauthorized("Пользователь не найден.");
                _token = _tokenSvc.BuildToken(_config["JwtKey"], _config["JwtIssuer"], validUser);
                if (_token == null) return Unauthorized("Ошибка генерации токена.");
                HttpContext.Session.SetString("Token", _token);
                return Ok("Вы успешно авторизованы.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Сервис временно недоступен.");
            }
        }
    }
}