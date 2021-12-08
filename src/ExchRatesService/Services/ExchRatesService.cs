using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using CentralExchRateService;
using System.Threading.Tasks;
using ExchRatesService.Helpers.Mapping;
using ExchRatesService.Repositories.Interfaces;
using System.Linq;
using System;

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
        public ExchRatesService(ILogger<ExchRatesService> logger, ICentralExchRateService service, IRatesManager dbManager)
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
        public override async Task<QuotesReply> GetCurrencyQuotes(QuotesRequest request, ServerCallContext context)
        {
            // Получение данных с ЦБ РФ через WCF
            var centerReply = await _centralExchService
                .GetCurrencyQuotesDescAsync(request.Time.ToDateTime());

            var response = new QuotesReply
            {
                Course = new CourseInfo
                {
                    Name = centerReply.Name,
                    Time = Timestamp.FromDateTime(request.Time.ToDateTime())
                }
            };

            foreach (var quoteDesc in centerReply.Valutes)
            {
                var valute = new QuoteInfo
                {
                    Id = quoteDesc.Id,
                    Name = quoteDesc.Name,
                    CharCode = quoteDesc.CharCode,
                    Nominal = quoteDesc.Nominal,
                    NumCode = quoteDesc.NumCode,
                    Value = float.Parse(quoteDesc.Value)
                };
                response.Valutes.Add(valute);
            }
            return await Task.FromResult(response);

        }

        /// <summary>
        ///     Получение информации о валютах и их кодах.
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<CodesReply> GetCurrencyCodes(Empty request, ServerCallContext context)
        {
            try
            {
                var codesDesc = await _centralExchService.GetCurrencyCodesDescAsync();
                var codesEnt = codesDesc.Map();
                if (codesDesc.Items.Length != _dbManager.Currencies.Count())
                {
                    await _dbManager.AddCodesAsync(codesEnt);
                    await _dbManager.SaveAsync();
                }

                var response = new CodesReply
                {
                    Code = new CodesInfo
                    {
                        Name = _dbManager.Codes.OrderBy(x=>x.Id).FirstOrDefault()?.Name 
                        ?? "Foreign Currency Market Lib"
                    }
                };

                foreach (var cur in _dbManager.Currencies)
                {
                    var item = cur.Map();
                    response.Items.Add(item);
                }
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                // TODO: Вернуть ошибку на клиента.
                _logger.LogError($"Ошибка при попытке получения данных {ex.Message}", ex);
                throw;
            }
            
        }
    }
}
