using ExchRatesFrontService.Services;
using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using gRPC_Client = ExchRatesSvc.ExchRates.ExchRatesClient;

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
        private readonly IExchRatesService _exchRatesService;
        private readonly gRPC_Client _client;

        public ExchRatesController(ILogger<ExchRatesController> logger,
            IExchRatesService exchRatesService, gRPC_Client client)
        {
            _client = client;
            _logger = logger;
            _exchRatesService = exchRatesService;
        }

        /// <summary>
        ///     Получение информации о значениях курсов (котировок) на заданный период.
        /// </summary>
        /// <param name="dateRequest">Дата формирования справки.</param>
        /// <returns>Список котировок для валют.</returns>
        [HttpPost("GetCurrencyQuotes")]
        public IEnumerable<QuotesInfo> GetCurrencyQuotes([FromQuery] DateTime dateRequest)
        {
            try
            {
                var req = new QuotesRequest
                {
                    Time = Timestamp.FromDateTime(dateRequest.ToUniversalTime()),
                };

                using (var call = _client.GetCurrencyQuotesAsync(req))
                {
                    var result = call.ResponseAsync.Result;
                    return result.Valutes.ToList();
                };

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
        public IEnumerable<CurrencyInfo> GetCurrencyCodes()
        {
            try
            {
                using (var call = _client.GetCurrencyCodesAsync(new Empty()))
                {
                    var result = call.ResponseAsync.Result;
                    return result.Items.ToList();
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
