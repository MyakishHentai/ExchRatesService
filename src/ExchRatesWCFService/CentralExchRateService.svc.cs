using ExchRatesWCFService.Models;
using ExchRatesWCFService.Services;
using ExchRatesWCFService.Services.Interfaces;
using NLog;
using System;
using System.Runtime.Serialization;

namespace ExchRatesWCFService
{
    /// <summary>
    ///     Сервис получение котировок и валют по средствам API ЦБ РФ.
    /// </summary>
    public class CentralExchRateService : ICentralExchRateService
    {
        private readonly IInfoService _infoService;
        private readonly NLog.ILogger _logger;

        public CentralExchRateService()
        {
            _infoService = new CbrInfoService();
            _logger = LogManager.GetLogger("fileLogger");
        }

        /// <summary>
        ///     Получение описания валют и их кодов.
        /// </summary>
        /// <returns>Описание валют.</returns>
        public CodesDesc GetCurrencyCodesDesc()
        {
            try
            {
                _logger.Info($"Вызов {nameof(GetCurrencyCodesDesc)}");
                return _infoService.GetCodesInfoXML<CodesDesc>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Ошибка десериализации {nameof(CodesDesc)}: {ex.Message}");
                throw new SerializationException(
                    "Не удалось получить информацию по кодам валют.", ex);
            }
        }


        /// <summary>
        ///     Получение котировок валют на заданный день.
        /// </summary>
        /// <param name="date">Дата формирования справки.</param>
        /// <returns>Значение котировок.</returns>
        public QuoteDesc GetCurrencyQuotesDesc(DateTime date)
        {
            try
            {
                _logger.Info($"Вызов {nameof(GetCurrencyQuotesDesc)}");
                return _infoService.GetDailyInfoXML<QuoteDesc>(date);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Ошибка десериализации {nameof(QuoteDesc)}:{ex.Message}");
                throw new SerializationException(
                    "Не удалось получить информацию по котировкам валют.", ex);
            }
        }
    }
}
