using ExchRatesWCFService.Services.Interfaces;
using System;
using System.Net.Http;
using System.Xml.Serialization;

namespace ExchRatesWCFService.Services
{
    public class CbrInfoService : IInfoService
    {
        private const string LinkCodesInfo = "http://www.cbr.ru/scripts/XML_valFull.asp";
        private const string LinkDaily = "https://www.cbr-xml-daily.ru/daily.xml";
        private const string ParamDaily = "date_req";

        /// <summary>
        ///     Получение данных по кодам валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        public T GetCodesInfoXML<T>() where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var xmlStream = client.GetStreamAsync(LinkCodesInfo).Result)
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        var code = serializer.Deserialize(xmlStream) as T;
                        return code;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Получение данных котировок валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        public T GetDailyInfoXML<T>(DateTime date) where T : class
        {
            var extLink = date == DateTime.MinValue ?
                LinkDaily :
                $@"{LinkDaily}?{ParamDaily}={date:dd/MM/yyyy}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var xmlStream = client.GetStreamAsync(extLink).Result)
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        var code = serializer.Deserialize(xmlStream) as T;
                        return code;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
