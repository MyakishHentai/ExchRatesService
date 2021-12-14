using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ExchRates.Common.Caching.Interfaces;
using ExchRatesFrontService.Models.Request;
using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using config = ExchRatesFrontService.Config.ServiceConfig;
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
        private readonly ICacheService _cache;
        private readonly Client _client;
        private readonly ILogger<ExchRatesController> _logger;


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
                if (dateRequest.Date > DateTime.Now ||
                    dateRequest.Date < new DateTime(1993, 1, 1))
                    throw new ValidationException("Дата формирования котировок должна находиться " +
                                                  $"в промежутке от {new DateTime(1993, 1, 1).Date} до {DateTime.Now.Date}");

                var date = dateRequest.Date.ToUniversalTime();
                
                string key = $"{dateRequest.Date:yyyy-MM-dd}_{nameof(GetCurrencyQuotes)}";

                if (_cache.Get<IEnumerable<QuoteInfo>>(key, out var cached))
                    return Ok(cached);

                var req = new QuotesRequest
                {
                    Time = Timestamp.FromDateTime(date)
                };

                using var call = _client.GetCurrencyQuotesAsync(req);
                var result = (await call.ResponseAsync).Valutes.ToList();

                _cache.Set<IEnumerable<QuoteInfo>>(key, result, Convert.ToInt32(config.CacheTime));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem($"{ex.Message}");
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
                string key = $"{DateTime.Today:yyyy-MM-dd}_{nameof(GetCurrencyCodes)}";

                if (_cache.Get<IEnumerable<CurrencyInfo>>(key, out var cached))
                    return Ok(cached);

                using var call = _client.GetCurrencyCodesAsync(new Empty());
                var result = (await call.ResponseAsync).Items.ToList();

                _cache.Set<IEnumerable<CurrencyInfo>>(key, result, Convert.ToInt32(config.CacheTime));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem($"{ex.Message}");
            }
        }
    }
}