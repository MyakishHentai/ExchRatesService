using ExchRatesService.Helpers.Mapping;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Services.Interfaces;
using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ExchRatesWCFService
{
    /// <summary>
    ///     Сервис получение котировок и валют по средствам API ЦБ РФ.
    /// </summary>
    public class CentralExchRateService : ICentralExchRateService
    {
        private readonly IBankService _bankService;
        private readonly IDataBaseService _baseService;
        private readonly ILogger _logger;

        public CentralExchRateService(ILogger logger, IBankService bank, IDataBaseService data)
        {
            _logger = logger;
            _bankService = bank;
            _baseService = data;
        }

        /// <summary>
        ///     Получение описания валют и их кодов.
        /// </summary>
        /// <returns>Описание валют.</returns>
        public async Task<MarketBank> GetCodesBankAsync()
        {
            try
            {
                _logger.Info($"Вызов {nameof(GetCodesBankAsync)}");
                var codesBank = _bankService.GetCodesInfoXML<MarketBank>();
                using (_baseService)
                {
                    if (_baseService.Codes.Count() != codesBank.Items.Length)
                    {
                        var toAdd = codesBank.Items.Map();
                        await _baseService.UpdateCodesAsync(toAdd);
                    }
                    var codesBase = await _baseService.Codes.AsNoTracking().ToListAsync();
                    var codes = codesBase.Map();
                    return new MarketBank
                    {
                        Name = _bankService.MarketName,
                        Items = codes.ToArray()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Ошибка при получении данных {nameof(MarketBank)}: {ex.Message}");
                throw new SerializationException(
                    "Не удалось получить информацию по кодам валют.", ex);
            }
        }


        /// <summary>
        ///     Получение котировок валют на заданный день.
        /// </summary>
        /// <param name="date">Дата формирования справки.</param>
        /// <returns>Значение котировок.</returns>
        public async Task<QuoteBank> GetQuotesBankAsync(DateTime date)
        {
            try
            {
                _logger.Info($"Вызов {nameof(GetQuotesBankAsync)}");
                var result = new QuoteBank
                {
                    Name = _bankService.MarketName
                };
                using (_baseService)
                {
                    QuoteBank quotesBank = null;
                    var quotesBase = _baseService.CodeQuotes
                        .Where(x => x.Quote.Date == date)
                        .AsNoTracking().ToList();

                    if (date != DateTime.Today && quotesBase.Any())
                    {
                        var quoteExist = quotesBase.Map();
                        result.Date = quoteExist.Date;
                        result.Valutes = quoteExist.Valutes;
                        return result;
                    }
                    // Если нет записей в базе.
                    quotesBank = _bankService
                            .GetDailyInfoXML<QuoteBank>(date);
                    await _baseService
                        .UpdateQuotesAsync(quotesBank.Map());
                    result.Date = date.ToString();
                    result.Valutes = quotesBank.Valutes;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Ошибка при получении данных {nameof(QuoteBank)}:{ex.Message}");
                throw new SerializationException(
                    "Не удалось получить информацию по котировкам валют.", ex);
            }
        }
    }
}
