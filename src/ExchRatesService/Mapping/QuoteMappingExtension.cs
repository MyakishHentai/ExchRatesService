using System.Collections.Generic;
using AutoMapper;
using CentralExchRateService;
using ExchRatesSvc;

namespace ExchRatesService.Mapping
{
    /// <summary>
    ///     Mapping сущностей, содержащих котировки валют.
    /// </summary>
    public static class QuoteMappingExtension
    {
        private static readonly Mapper Mapper;

        static QuoteMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CodeQuoteBank, QuoteInfo>()
                        .ForMember(x => x.Value, opt => opt.MapFrom(src => float.Parse(src.Value)))
                        .ReverseMap();
                });
            Mapper = new Mapper(config);
        }


        /// <summary>
        ///     Преобразование <see cref="CodeBank" /> для типов protobuf.
        /// </summary>
        /// <param name="this">Тип контракта WCF.</param>
        /// <returns>Тип контракта gRPC.</returns>
        public static IEnumerable<QuoteInfo> Map(this IEnumerable<CodeQuoteBank> @this)
        {
            return Mapper.Map<IEnumerable<CodeQuoteBank>, IEnumerable<QuoteInfo>>(@this);
        }
    }
}