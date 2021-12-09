using CentralExchRateService;
using ExchRatesService.Helpers.Mapping;
using ExchRatesService.Repositories.Interfaces;
using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
        private readonly IRatesManager _dbManager;
        public ExchRatesService(ILogger<ExchRatesService> logger, ICentralExchRateService service,
            IRatesManager dbManager)
        {
            _logger = logger;
            _centralExchService = service;
            _dbManager = dbManager;
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
                var quotesDesc = await _centralExchService
                    .GetCurrencyQuotesDescAsync(request.Time.ToDateTime());
                using (_dbManager)
                {
                    var quotesEnt = quotesDesc.Map();
                    // TODO: TaskScheduler  
                    await _dbManager.AddQuotesAsync(quotesEnt);
                    await _dbManager.SaveAsync();
                    // Полученное время формируется только при ответе, т.к.
                    // существуют даты, когда информации по ним нет => 
                    // возвращается текущая дата.
                    var trueDate = DateTime.Parse(quotesDesc.Date)
                        .ToUniversalTime();
                    response.Course = new CourseInfo
                    {
                        Name = quotesDesc.Name,
                        Time = Timestamp.FromDateTime(trueDate)
                    };

                    foreach (var quote in _dbManager.QuotesCurrencies
                        .Where(x => x.QuoteId == trueDate.ToLocalTime()))
                    {
                        var item = quote.Map();
                        response.Valutes.Add(item);
                    }
                }
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при попытке получения данных:\r\n{ex.Message}", ex);
                throw new Exception("Сервис временно недоступен");
            }
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
                var codesDesc = await _centralExchService.GetCurrencyCodesDescAsync();
                var codesEnt = codesDesc.Map();
                using (_dbManager)
                {
                    await _dbManager.AddCodesAsync(codesEnt);
                    await _dbManager.SaveAsync();

                    response.Code = new CodesInfo
                    {
                        Name = _dbManager.Codes.OrderBy(x => x.Id).FirstOrDefault()?.Name
                        ?? "Foreign Currency Market Lib"
                    };

                    foreach (var cur in _dbManager.Currencies)
                    {
                        var item = cur.Map();
                        response.Items.Add(item);
                    }
                }
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при попытке получения данных:\r\n{ex.Message}", ex);
                throw new Exception("Сервис временно недоступен");
            }
        }
    }
}
