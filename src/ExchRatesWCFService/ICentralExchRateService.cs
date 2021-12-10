using ExchRatesWCFService.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ExchRatesWCFService
{
    /// <summary>
    ///     Контракт работы с API ЦБ РФ и БД.
    /// </summary>
    [ServiceContract]
    public interface ICentralExchRateService
    {
        /// <summary>
        ///     Получение описания валют и их кодов.
        /// </summary>
        /// <returns>Описание валют.</returns>
        [OperationContract]
        Task<MarketBank> GetCodesBankAsync();


        /// <summary>
        ///     Получение котировок валют на заданный день.
        /// </summary>
        /// <param name="date">Дата формирования справки.</param>
        /// <returns>Значение котировок.</returns>
        [OperationContract]
        Task<QuoteBank> GetQuotesBankAsync(DateTime date);
    }
}
