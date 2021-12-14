using AutoMapper;
using CentralExchRateService;
using ExchRatesSvc;
using System.Collections.Generic;

namespace ExchRatesService.Mapping
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
                   cfg.CreateMap<CodeBank, CurrencyInfo>()
                   .ForMember(x => x.EngName, opt => opt.MapFrom(src => src.EngName ?? ""))
                   .ForMember(x => x.ParentCode, opt => opt.MapFrom(src => src.ParentCode ?? ""));
               });
            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     Преобразование <see cref="CodeBank"/> для типов protobuf.
        /// </summary>
        /// <param name="this">Тип контракта WCF.</param>
        /// <returns>Тип контракта gRPC.</returns>
        public static IEnumerable<CurrencyInfo> Map(this IEnumerable<CodeBank> @this)
            => _mapper.Map<IEnumerable<CodeBank>, IEnumerable<CurrencyInfo>>(@this);
    }
}
