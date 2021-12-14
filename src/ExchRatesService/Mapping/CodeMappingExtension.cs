using System.Collections.Generic;
using AutoMapper;
using CentralExchRateService;
using ExchRatesSvc;

namespace ExchRatesService.Mapping
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
                    cfg.CreateMap<CodeBank, CurrencyInfo>()
                        .ForMember(x => x.EngName, opt => opt.MapFrom(src => src.EngName ?? ""))
                        .ForMember(x => x.ParentCode, opt => opt.MapFrom(src => src.ParentCode ?? ""));
                });
            Mapper = new Mapper(config);
        }

        /// <summary>
        ///     Преобразование <see cref="CodeBank" /> для типов protobuf.
        /// </summary>
        /// <param name="this">Тип контракта WCF.</param>
        /// <returns>Тип контракта gRPC.</returns>
        public static IEnumerable<CurrencyInfo> Map(this IEnumerable<CodeBank> @this)
        {
            return Mapper.Map<IEnumerable<CodeBank>, IEnumerable<CurrencyInfo>>(@this);
        }
    }
}