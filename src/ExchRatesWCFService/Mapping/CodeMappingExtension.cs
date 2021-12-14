using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Models.Entity;

namespace ExchRatesWCFService.Mapping
{
    /// <summary>
    ///     Mapping сущностей, содежащих информацию по кодам валют.
    /// </summary>
    public static class CodeMappingExtension
    {
        private static readonly Mapper Mapper;

        static CodeMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CodeBank, Code>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));
                    cfg.CreateMap<Code, CodeBank>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));

                    cfg.CreateMap<QuoteBank, Quote>()
                        .ForMember(x => x.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date)))
                        .ReverseMap();

                    cfg.CreateMap<Code, CodeQuoteBank>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));
                    cfg.CreateMap<CodeQuoteBank, Code>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));

                    cfg.CreateMap<Quote, QuoteBank>()
                        .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString(CultureInfo.CurrentCulture)))
                        .ReverseMap();
                });
            Mapper = new Mapper(config);
        }

        /// <summary>
        ///     Преобразование к сущностям контрактов <see cref="CodeBank" />.
        /// </summary>
        /// <param name="this">Коды валют из БД.</param>
        /// <returns>Сущности для контрактов.</returns>
        public static IEnumerable<CodeBank> Map(this IEnumerable<Code> @this)
        {
            return Mapper.Map<IEnumerable<Code>, IEnumerable<CodeBank>>(@this);
        }

        /// <summary>
        ///     Преобразование к сущностям для БД.
        /// </summary>
        /// <param name="this">Коды валют ЦБ РФ.</param>
        /// <returns>Сущности для БД.</returns>
        public static IEnumerable<Code> Map(this IEnumerable<CodeBank> @this)
        {
            return Mapper.Map<IEnumerable<CodeBank>, IEnumerable<Code>>(@this);
        }

        /// <summary>
        ///     Преобразование к сущностям для БД.
        /// </summary>
        /// <param name="this">Котировки валют ЦБ РФ.</param>
        /// <returns>Сущности для БД.</returns>
        public static IEnumerable<CodeQuote> Map(this QuoteBank @this)
        {
            if (@this?.Valutes is null)
                return null;
            var result = new List<CodeQuote>(@this.Valutes.Length);
            foreach (var valute in @this.Valutes)
            {
                var code = Mapper.Map<Code>(valute);
                var quote = Mapper.Map<Quote>(@this);
                var valueStr = @this.Valutes.First(x => x.Id == code.Id).Value;

                var codeQuote = new CodeQuote
                {
                    CodeId = code.Id,
                    Code = code,
                    Quote = quote,
                    Value = float.Parse(valueStr)
                };

                result.Add(codeQuote);
            }
            return result;
        }

        /// <summary>
        ///     Преобразование к сущностям контрактов <see cref="QuoteBank" />.
        /// </summary>
        /// <param name="this">Котировки валют из БД.</param>
        /// <returns>Сущности для контрактов.</returns>
        public static QuoteBank Map(this IEnumerable<CodeQuote> @this)
        {
            if (@this is null || !@this.Any())
                return null;
            var quote = @this.First().Quote;
            var result = new QuoteBank
            {
                Name = quote.Name,
                Date = quote.Date.ToString(CultureInfo.CurrentCulture)
            };

            var codesQuote = new List<CodeQuoteBank>(@this.Count());

            foreach (var item in @this)
            {
                var code = Mapper.Map<CodeQuoteBank>(item.Code);
                code.Value = item.Value.ToString();
                codesQuote.Add(code);
            }

            result.Valutes = codesQuote.ToArray();

            return result;
        }
    }
}