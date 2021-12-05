using ExchRatesWCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;

namespace ExchRatesWCFService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "CentralExchRateService" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы CentralExchRateService.svc или CentralExchRateService.svc.cs в обозревателе решений и начните отладку.
    public class CentralExchRateService : ICentralExchRateService
    {
        private const string LinkCodesInfo = "http://www.cbr.ru/scripts/XML_valFull.asp";
        private const string LinkDaily = "https://www.cbr-xml-daily.ru/daily.xml";
        private const string ParamDaily = "date_req";

        public void Update()
        {
            throw new NotImplementedException();
        }

        public CodesDesc GetCurrencyCodes()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var xmlStream = client.GetStreamAsync(LinkCodesInfo).Result)
                    {
                        var serializer = new XmlSerializer(typeof(CodesDesc));
                        var code = serializer.Deserialize(xmlStream) as CodesDesc;
                        return code;
                    }
                }
            }
            catch (Exception ex)
            {
                // log: error
                return null;
            }
        }


        /// <summary>
        ///     Получение котировок валют с сайта ЦБ в форме XML.
        /// </summary>
        /// <param name="date">Дата отслеживания.</param>
        /// <returns>Описание котировок.</returns>
        public QuoteDesc GetCurrencyQuotes(DateTime date)
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
                        var serializer = new XmlSerializer(typeof(QuoteDesc));
                        var quote = serializer.Deserialize(xmlStream) as QuoteDesc;
                        return quote;
                    }
                }
            }
            catch (Exception ex)
            {
                // log: error
                return null;
            }
        }
    }
}
