using CentralExchRateService;
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
                var codesDesc = await _centralExchService.GetCodesBankAsync();

                response.Code = new CodesInfo
                {
                    Name = codesDesc.Name                    
                };
                response.Items.AddRange(codesDesc.Items.Map());

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
                //var quotesDesc = await _centralExchService
                //    .GetCurrencyQuotesDescAsync(request.Time.ToDateTime().ToLocalTime());
                
                //    // Полученное время формируется только при ответе, т.к.
                //    // существуют даты, когда информации по ним нет => 
                //    // возвращается текущая дата.
                //    var trueDate = DateTime.Parse(quotesDesc.Date)
                //        .ToUniversalTime();
                //    response.Course = new CourseInfo
                //    {
                //        Name = quotesDesc.Name,
                //        Time = Timestamp.FromDateTime(trueDate)
                //    };

                //    foreach (var quote in _dbManager.QuotesCurrencies
                //        .Include(x=>x.Quote).Include(x=>x.Code)
                //        .AsNoTracking()
                //        .Where(x => x.Quote.Date == trueDate.ToLocalTime()))
                //    {
                //        var item = quote.Map();
                //        response.Valutes.Add(item);
                //    }
                //}
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
