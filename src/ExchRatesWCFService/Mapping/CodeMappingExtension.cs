using AutoMapper;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchRatesService.Helpers.Mapping
{
    /// <summary>
    ///     Mapping сущностей, содежащих информацию по кодам валют.
    /// </summary>
    public static class CodeMappingExtension
    {
        private static readonly Mapper _mapper;
        static CodeMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CodeBank, Code>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));
                    cfg.CreateMap<Code, CodeBank> ()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));

                    cfg.CreateMap<QuoteBank, Quote>()
                        .ForMember(x => x.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date)))
                        .ReverseMap();

                    cfg.CreateMap<Code, CodeQuoteBank>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));
                    cfg.CreateMap<CodeQuoteBank, Code>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id.Trim()));

                    cfg.CreateMap<Quote, QuoteBank>()
                        .ForMember(x => x.Date, opt => opt.MapFrom(src => src.Date.ToString()))
                        .ReverseMap();
                });
            _mapper = new Mapper(config);
        }

        public static IEnumerable<CodeBank> Map(this IEnumerable<Code> @this)
            => _mapper.Map<IEnumerable<Code>, IEnumerable<CodeBank>>(@this);

        public static IEnumerable<Code> Map(this IEnumerable<CodeBank> @this)
            => _mapper.Map<IEnumerable<CodeBank>, IEnumerable<Code>>(@this);

        /// <summary>
        ///     Преобразование к сущностям для БД.
        /// </summary>
        /// <param name="this">Котировки валют ЦБ РФ.</param>
        /// <returns>Сущности для БД.</returns>
        public static IEnumerable<CodeQuote> Map(this QuoteBank @this)
        {
            if (@this is null || @this.Valutes is null)
                return null;
            var result = new List<CodeQuote>(@this.Valutes.Length);
            foreach (var valute in @this.Valutes)
            {
                var code = _mapper.Map<Code>(valute);
                var quote = _mapper.Map<Quote>(@this);
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
        ///     Преобразование из сущностей для типа контракта <see cref="QuoteBank"/>.
        /// </summary>
        /// <param name="this">Котировки валют.</param>
        /// <returns>Объект контракта WCF.</returns>
        public static QuoteBank Map(this IEnumerable<CodeQuote> @this)
        {
            if (@this is null || !@this.Any())
                return null;
            var quote = @this.First().Quote;
            var result = new QuoteBank
            {
                Name = quote.Name,
                Date = quote.Date.ToString()
            };

            var codesQuote = new List<CodeQuoteBank>(@this.Count());

            foreach (var item in @this)
            {
                var code = _mapper.Map<CodeQuoteBank>(item.Code);
                code.Value = item.Value.ToString();
                codesQuote.Add(code);
            }
            result.Valutes = codesQuote.ToArray();

            return result;
        }
    }
}
