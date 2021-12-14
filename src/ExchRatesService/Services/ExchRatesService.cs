using CentralExchRateService;
using ExchRatesService.Helpers.Mapping;
using ExchRatesService.Mapping;
using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesService.Services
{
    /// <summary>
    ///     Сервис, отвечающий за получение котировок валют и их кодов.
    ///     Обращается за информацией через WCF к БД.
    /// </summary>
    public class ExchRatesService : ExchRatesSvc.ExchRates.ExchRatesBase
    {
        private readonly ILogger<ExchRatesService> _logger;
        private readonly ICentralExchRateService _centralExchService;
        public ExchRatesService(ILogger<ExchRatesService> logger, ICentralExchRateService service)
        {
            _logger = logger;
            _centralExchService = service;
        }


        /// <summary>
        ///     Получение информации о валютах и их кодах.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<CodesReply> GetCurrencyCodes(Empty request, ServerCallContext context)
        {
            var response = new CodesReply();
            try
            {
                _logger.LogInformation($"Вызов {nameof(GetCurrencyCodes)}...");
                var codesDesc = await _centralExchService.GetCodesBankAsync();

                response.Code = new CodesInfo
                {
                    Name = codesDesc.Name                    
                };
                response.Items.AddRange(codesDesc.Items.Map());

                _logger.LogInformation($"Успешное извлечение информации по кодам валют.");
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при попытке получения данных:\r\n{ex.Message}", ex);
                throw new Exception("Ошибка при попытке получения данных.");
            }
        }


        /// <summary>
        ///     Получение информации о значениях курсов (котировок) на заданный период.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<QuotesReply> GetCurrencyQuotes(QuotesRequest request,
            ServerCallContext context)
        {
            var response = new QuotesReply();
            try
            {
                _logger.LogInformation($"Вызов {nameof(GetCurrencyQuotes)}...");
                var quotesDesc = await _centralExchService
                    .GetQuotesBankAsync(request.Time.ToDateTime().ToLocalTime());

                var utcDate = DateTime.Parse(quotesDesc.Date)
                    .ToUniversalTime();

                response.Course = new CourseInfo
                {
                    Name = quotesDesc.Name,
                    Time = Timestamp.FromDateTime(utcDate)
                };
                response.Valutes.AddRange(quotesDesc.Valutes.Map());

                _logger.LogInformation($"Успешное извлечение информации по котировкам валют.");
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при попытке получения данных:\r\n{ex.Message}", ex);
                throw new Exception("Ошибка при попытке получения данных.");
            }
        }

    }
}
