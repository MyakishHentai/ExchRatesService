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
                   .ReverseMap();
               });
            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     WCF to Entity mapping.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Сущность ORM.</returns>
        public static ICollection<CurrencyInfo> Map(this ICollection<CodeBank> @this)
            => _mapper.Map<ICollection<CodeBank>, ICollection<CurrencyInfo>>(@this);
    }
}
