using ExchRates.Common.Caching.Interfaces;
using ExchRatesFrontService.Config;
using ExchRatesFrontService.Models.Request;
using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client = ExchRatesSvc.ExchRates.ExchRatesClient;

namespace ExchRatesFrontService.Controllers
{
    /// <summary>
    ///     Контроллер, отвечающий за получение котировок валют и их кодов.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ExchRatesController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ExchRatesController> _logger;
        private readonly Client _client;
        private readonly ICacheService _cache;


        public ExchRatesController(
            ILogger<ExchRatesController> logger,
            Client client,
            ICacheService cache)
        {
            _client = client;
            _logger = logger;
            _cache = cache;
        }


        /// <summary>
        ///     Получение информации о значениях курсов (котировок) на заданный период.
        /// </summary>
        /// <param name="dateRequest">Дата формирования справки.</param>
        /// <returns>Список котировок для валют.</returns>
        [HttpPost("GetCurrencyQuotes")]
        public async Task<IActionResult> GetCurrencyQuotes([FromQuery] DateRequest dateRequest)
        {
            try
            {
                var date = dateRequest.Date.ToUniversalTime();

                if (_cache.Get<IEnumerable<QuoteInfo>>(
                    $"{nameof(GetCurrencyQuotes)}_{date}", out var cached))
                    return Ok(cached);

                var req = new QuotesRequest
                {
                    Time = Timestamp.FromDateTime(date),
                };

                using var call = _client.GetCurrencyQuotesAsync(req);
                var result = (await call.ResponseAsync).Valutes.ToList();

                _cache.Set<IEnumerable<QuoteInfo>>($"{nameof(GetCurrencyQuotes)}_{date}", 
                    result, Convert.ToInt32(ServiceConfig.CacheTime));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Сервис временно недоступен");
            }
        }

        /// <summary>
        ///     Получение информации о валютах и их кодах.
        /// </summary>
        /// <returns>Список кодов для валют.</returns>
        [HttpPost("GetCurrencyCodes")]
        public async Task<IActionResult> GetCurrencyCodes([FromQuery] BaseRequest baseRequest)
        {
            try
            {
                if (_cache.Get<IEnumerable<CurrencyInfo>>($"{nameof(GetCurrencyCodes)}_{DateTime.Today}",
                      out var cached))
                {
                    return Ok(cached);
                }
                using var call = _client.GetCurrencyCodesAsync(new Empty());
                var result = (await call.ResponseAsync).Items.ToList();

                _cache.Set<IEnumerable<CurrencyInfo>>($"{nameof(GetCurrencyCodes)}_{DateTime.Today}",
                    result, Convert.ToInt32(ServiceConfig.CacheTime));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Сервис временно недоступен");
            }
        }


        //[HttpPost("Login")]
        //public bool Login(
        //    [FromQuery] BaseRequest baseRequest,
        //    [FromForm] LoginRequest request)
        //{
        //    if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
        //        return ErrorAuthentication;

        //    var validUser = _userRep.GetUser(request.UserName, request.Password);

        //    if (validUser != null)
        //    {
        //        _token = _tokenSvc.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
        //        if (_token != null)
        //        {
        //            HttpContext.Session.SetString("Token", _token);
        //            return SuccessAuthentication;
        //        }
        //        return ErrorAuthentication;
        //    }
        //    return ErrorAuthentication;
        //}

        //private bool ErrorAuthentication 
        //{
        //    get
        //    {
        //        _logger.LogError("Авторизация/аутентификация не пройдена.");
        //        HttpContext.Response.StatusCode = 403;
        //        return false;
        //    }
        //}

        //private bool SuccessAuthentication
        //{
        //    get 
        //    {
        //        _logger.LogError("Авторизация/аутентификация успешно пройдена.");
        //        HttpContext.Response.StatusCode = 200;
        //        return true;
        //    }
        //}


    }
}
