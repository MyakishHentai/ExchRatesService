using System;
using System.Net.Http;
using System.Xml.Serialization;
using ExchRatesWCFService.Services.Interfaces;

namespace ExchRatesWCFService.Services
{
    public class BankInfoService : IBankService
    {
        private const string LINK_CODES_INFO = "http://www.cbr.ru/scripts/XML_valFull.asp";
        private const string LINK_DAILY = "https://www.cbr.ru/scripts/XML_daily.asp";
        private const string PARAM_DAILY = "date_req";
        private const string MARKET = "Foreign Currency Market Lib";
        public string MarketName => MARKET;


        /// <summary>
        ///     Получение данных по кодам валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        public T GetCodesInfoXml<T>() where T : class
        {
            using (var client = new HttpClient())
            {
                using (var xmlStream = client.GetStreamAsync(LINK_CODES_INFO).Result)
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var code = serializer.Deserialize(xmlStream) as T;
                    return code;
                }
            }
        }

        /// <summary>
        ///     Получение данных котировок валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        public T GetDailyInfoXml<T>(DateTime date) where T : class
        {
            var extLink = date == DateTime.MinValue ? LINK_DAILY : $@"{LINK_DAILY}?{PARAM_DAILY}={date:dd/MM/yyyy}";

            using (var client = new HttpClient())
            {
                using (var xmlStream = client.GetStreamAsync(extLink).Result)
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var code = serializer.Deserialize(xmlStream) as T;
                    return code;
                }
            }
        }
    }
}