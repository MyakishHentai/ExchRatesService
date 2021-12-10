using ExchRatesService.Helpers.Mapping;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Models.Entity;
using ExchRatesWCFService.Services;
using ExchRatesWCFService.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
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

        private readonly NLog.ILogger _logger;

        public CentralExchRateService()
        {
            _logger = LogManager.GetLogger("fileLogger");

            _bankService = new BankInfoService();
            _baseService = new DataBaseInfoService(_logger);
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
                // Проверям БД.
                using (_baseService)
                {
                    var codesBase = await _baseService.Codes.AsNoTracking().ToListAsync();
                    // Проверяем на совпдаение данных.
                    while (_baseService.Codes.Count() != codesBank.Items.Length)
                    {
                        var toAdd = codesBank.Items.Map();
                        await _baseService.AddCodesAsync(toAdd);
                        await _baseService.SaveAsync();
                    }
                    codesBase = await _baseService.Codes.AsNoTracking().ToListAsync();
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
                _logger.Error(ex, $@"Ошибка десериализации {nameof(MarketBank)}: {ex.Message}");
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
                return await Task.Run(() => _bankService.GetDailyInfoXML<QuoteBank>(date));
            } 
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Ошибка десериализации {nameof(QuoteBank)}:{ex.Message}");
                throw new SerializationException(
                    "Не удалось получить информацию по котировкам валют.", ex);
            }
        }
    }
}
