using ExchRatesSvc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using CentralExchRateService;
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
                    Id = quoteDesc.ID,
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
            using (var client = new CentralExchRateServiceClient())
            {
                var wcfReply = await client.GetCurrencyCodesDescAsync();
                var response = new CodesReply
                {
                    Code = new CodesInfo
                    {
                        Name = wcfReply.Name
                    }
                };
                foreach (var codeDesc in wcfReply.Items)
                {
                    var item = new CurrencyInfo
                    {
                        Id = codeDesc.ID,
                        Name = codeDesc.Name,
                        EngName = codeDesc.EngName,
                        Nominal = codeDesc.Nominal,
                        ParentCode = codeDesc.ParentCode,
                        NumCode = codeDesc.NumCode,
                        CharCode = codeDesc.CharCode
                    };
                    response.Items.Add(item);
                }
                return await Task.FromResult(response);
            };
        }
    }
}
