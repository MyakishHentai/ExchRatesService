using ExchRatesWCFService.Models;
using ExchRatesWCFService.Services;
using ExchRatesWCFService.Services.Interfaces;
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

        public CentralExchRateService()
        {
            _infoService = new CbrInfoService();
        }        

        public CodesDesc GetCurrencyCodesDesc()
        {
            try
            {
                //logger
                return _infoService.GetCodesInfoXML<CodesDesc>();
            }
            catch (Exception ex)
            {
                //logger
                throw new SerializationException(
                    "Не удалось получить информацию по кодам валют.", ex);
            }
        }


        /// <summary>
        ///     Получение котировок валют с сайта ЦБ в форме XML.
        /// </summary>
        /// <param name="date">Дата отслеживания.</param>
        /// <returns>Описание котировок.</returns>
        public QuoteDesc GetCurrencyQuotesDesc(DateTime date)
        {
            try
            {
                return _infoService.GetDailyInfoXML<QuoteDesc>(date);
            }
            catch (Exception ex)
            {
                // log: error
                throw new SerializationException(
                    "Не удалось получить информацию по кодам валют.", ex);

            }
        }
    }
}
