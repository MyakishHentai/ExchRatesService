using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<ExchRatesController> _logger;
        private readonly Client _client;

        public ExchRatesController(ILogger<ExchRatesController> logger, Client client)
        {
            _client = client;
            _logger = logger;
        }

        /// <summary>
        ///     Получение информации о значениях курсов (котировок) на заданный период.
        /// </summary>
        /// <param name="dateRequest">Дата формирования справки.</param>
        /// <returns>Список котировок для валют.</returns>
        [HttpPost("GetCurrencyQuotes")]
        public async Task<IEnumerable<QuoteInfo>> GetCurrencyQuotes([FromQuery] DateTime dateRequest)
        {
            try
            {
                var req = new QuotesRequest
                {
                    Time = Timestamp.FromDateTime(dateRequest.ToUniversalTime()),
                };

                using var call = _client.GetCurrencyQuotesAsync(req);
                return (await call.ResponseAsync).Valutes.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     Получение информации о валютах и их кодах.
        /// </summary>
        /// <returns>Список кодов для валют.</returns>
        [HttpPost("GetCurrencyCodes")]
        public async Task<IEnumerable<CurrencyInfo>> GetCurrencyCodes()
        {
            try
            {
                using var call = _client.GetCurrencyCodesAsync(new Empty());
                return (await call.ResponseAsync).Items.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
