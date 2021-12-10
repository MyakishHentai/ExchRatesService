//using AutoMapper;
//using CentralExchRateService;
//using ExchRatesService.Models;
//using ExchRatesSvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ExchRatesService.Helpers.Mapping
//{
//    /// <summary>
//    ///     Mapping сущностей, содержащих котировки валют.
//    /// </summary>
//    public static class QuoteMappingExtension
//    {
//        private static readonly Mapper _mapper;
//        static QuoteMappingExtension()
//        {
//            var local = new QuoteDesc();
//            var config = new MapperConfiguration(
//                cfg =>
//                {
//                    // WCF => Entity
//                    cfg.CreateMap<QuoteDesc, Quote>()
//                        .ForMember(x => x.Valutes, opt => opt.Ignore())
//                        .ForMember(x => x.Date, opt => opt.MapFrom(src => DateTime.Parse(src.Date)));
//                    cfg.CreateMap<CurrencyQuoteDesc, Code>();

//                    // Entity => gRPC
//                    cfg.CreateMap<Code, QuoteInfo>();
//                    cfg.CreateMap<CodeQuote, QuoteInfo>()
//                        .ForMember(x => x.Value, opt => opt.MapFrom(src => src.Value))
//                        .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Code.Id))
//                        .ForMember(x => x.NumCode, opt => opt.MapFrom(src => src.Code.NumCode))
//                        .ForMember(x => x.CharCode, opt => opt.MapFrom(src => src.Code.CharCode))
//                        .ForMember(x => x.Nominal, opt => opt.MapFrom(src => src.Code.Nominal))
//                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Code.Name));
//                });
//            _mapper = new Mapper(config);
//        }

//        /// <summary>
//        ///     WCF to Entity mapping.
//        /// </summary>
//        /// <param name="this"></param>
//        /// <returns>Сущность ORM.</returns>
//        public static ICollection<CodeQuote> Map(this QuoteDesc @this)
//        {
//            var quote = _mapper.Map<Quote>(@this);
//            var codes = _mapper.Map<
//                CurrencyQuoteDesc[], 
//                ICollection<Code>>(@this.Valutes);

//            var rates = new List<CodeQuote>();
//            foreach (var code in codes)
//            {
//                var valueStr = @this.Valutes.FirstOrDefault(x => x.Id == code.Id)?.Value;

//                var rate = new CodeQuote
//                {
//                    Quote = quote,
//                    Code = code,
//                    CodeId = code.Id,
//                    Value = float.Parse(valueStr)
//                };
//                rates.Add(rate);
//            }
//            return rates;
//        }


//        /// <summary>
//        ///     Entity to gRPC mapping. 
//        /// </summary>
//        /// <param name="this"></param>
//        /// <returns>Сущность контракта protobuf.</returns>
//        public static QuoteInfo Map(this CodeQuote @this) => _mapper.Map<QuoteInfo>(@this);
//    }
//}
