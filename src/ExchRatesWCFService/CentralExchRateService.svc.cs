using AutoMapper;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Models.Entities;
using ExchRatesWCFService.Models.Mapping;
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
    /// <summary>
    ///     Сервис получение котировок и валют по средствам API ЦБ РФ.
    /// </summary>
    public class CentralExchRateService : ICentralExchRateService
    {
        private const string LinkCodesInfo = "http://www.cbr.ru/scripts/XML_valFull.asp";
        private const string LinkDaily = "https://www.cbr-xml-daily.ru/daily.xml";
        private const string ParamDaily = "date_req";
        
        public void Update()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Проверка на существование в БД.
        /// </summary>
        /// <returns>Значение существования.</returns>
        private bool СheckExistence()
        {
            return false;
        }
        
        private CodesDesc GetCurrencyCodesFromDB()
        {
            try
            {
                return new CodesDesc();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CodesDesc GetCurrencyCodes()
        {
            try
            {
                if (!СheckExistence())
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (var xmlStream = client.GetStreamAsync(LinkCodesInfo).Result)
                        {
                            var serializer = new XmlSerializer(typeof(CodesDesc));
                            var code = serializer.Deserialize(xmlStream) as CodesDesc;
                            var codeEntity = code.Map();
                            // Mapping
                            using (var context = new ExchRatesContext())
                            {
                                var codeEntity1 = code.Map();
                                context.Codes.Add(codeEntity);
                                context.SaveChanges();
                            }
                            return code;
                        }
                    }

                }
                return GetCurrencyCodesFromDB();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private QuoteDesc GetCurrencyQuotesFromDB()
        {
            try
            {
                return new QuoteDesc();

            }
            catch (Exception ex)
            {
                throw ex;
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
                if (!СheckExistence())
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
                return GetCurrencyQuotesFromDB();
            }
            catch (Exception ex)
            {
                // log: error
                return null;
            }
        }
    }
}
