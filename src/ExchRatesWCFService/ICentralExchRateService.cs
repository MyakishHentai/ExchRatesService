using ExchRatesWCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ExchRatesWCFService
{
    /// <summary>
    ///     Контракт работы с API ЦБ РФ и БД.
    /// </summary>
    [ServiceContract]
    public interface ICentralExchRateService
    {
        /// <summary>
        ///     Обновление БД.
        /// </summary>
        [OperationContract]
        void Update();

        /// <summary>
        ///     Получение котировок валют на заданный день.
        /// </summary>
        /// <param name="date">Дата формирования справки.</param>
        /// <returns>Значение котировок.</returns>
        [OperationContract]
        QuoteDesc GetCurrencyQuotes(DateTime date);


        /// <summary>
        ///     Получение описания валют и их кодов.
        /// </summary>
        /// <returns>Описание валют.</returns>
        [OperationContract]
        CodesDesc GetCurrencyCodes();
    }
}
