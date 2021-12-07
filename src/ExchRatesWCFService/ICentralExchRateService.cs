using ExchRatesWCFService.Models;
using System;
using System.ServiceModel;

namespace ExchRatesWCFService
{
    /// <summary>
    ///     Контракт работы с API ЦБ РФ и БД.
    /// </summary>
    [ServiceContract]
    public interface ICentralExchRateService
    {
        /// <summary>
        ///     Получение котировок валют на заданный день.
        /// </summary>
        /// <param name="date">Дата формирования справки.</param>
        /// <returns>Значение котировок.</returns>
        [OperationContract]
        QuoteDesc GetCurrencyQuotesDesc(DateTime date);


        /// <summary>
        ///     Получение описания валют и их кодов.
        /// </summary>
        /// <returns>Описание валют.</returns>
        [OperationContract]
        CodesDesc GetCurrencyCodesDesc();
    }
}
