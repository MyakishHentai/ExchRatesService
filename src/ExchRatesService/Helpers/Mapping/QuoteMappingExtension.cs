using AutoMapper;
using CentralExchRateService;
using ExchRatesService.Models;
using ExchRatesSvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchRatesService.Helpers.Mapping
{
    /// <summary>
    ///     Mapping сущностей, содержащих котировки валют.
    /// </summary>
    public static class QuoteMappingExtension
    {
        private static readonly Mapper _mapper;
        static QuoteMappingExtension()
        {
            var local = new QuoteDesc();
            var config = new MapperConfiguration(
                cfg =>
                {
                    // WCF => Entity
                    cfg.CreateMap<QuoteDesc, Quote>()
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => DateTime.Parse(src.Date)))
                        .ForMember(x => x.Valutes, opt => opt.Ignore());
                    cfg.CreateMap<CurrencyQuoteDesc, CurrencyCodes>();
                    // Entity => gRPC
                    cfg.CreateMap<CurrencyCodes, QuoteInfo>();
                    cfg.CreateMap<QuoteCurrency, QuoteInfo>()
                        .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Code.Id))
                        .ForMember(x => x.NumCode, opt => opt.MapFrom(src => src.Code.NumCode))
                        .ForMember(x => x.CharCode, opt => opt.MapFrom(src => src.Code.CharCode))
                        .ForMember(x => x.Nominal, opt => opt.MapFrom(src => src.Code.Nominal))
                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Code.Name));
                });
            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     WCF to Entity mapping.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Сущность ORM.</returns>
        public static ICollection<QuoteCurrency> Map(this QuoteDesc @this)
        {
            var quote = _mapper.Map<Quote>(@this);
            var codes = _mapper
                .Map<CurrencyQuoteDesc[], ICollection<CurrencyCodes>>(@this.Valutes);

            var rates = new List<QuoteCurrency>();
            foreach (var code in codes)
            {
                var valueStr = @this.Valutes.FirstOrDefault(x => x.Id == code.Id)?.Value;

                var rate = new QuoteCurrency
                {
                    Quote = quote,
                    QuoteId = quote.Id,
                    Code = code,
                    CodeId = code.Id,
                    Value = float.Parse(valueStr)
                };
                rates.Add(rate);
            }
            return rates;
        }


        /// <summary>
        ///     Entity to gRPC mapping. 
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Сущность контракта protobuf.</returns>
        public static QuoteInfo Map(this QuoteCurrency @this) => _mapper.Map<QuoteInfo>(@this);
    }
}
