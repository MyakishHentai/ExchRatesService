using AutoMapper;
using CentralExchRateService;
using ExchRatesService.Models;
using ExchRatesSvc;
using System.Collections.Generic;

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
                    // WCF => Entity
                    cfg.CreateMap<CurrencyCodesDesc, CurrencyCodes>();
                    // Entity => gRPC
                    cfg.CreateMap<CurrencyCodes, CurrencyInfo>();
                });
            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     WCF to Entity mapping.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Сущность ORM.</returns>
        public static Codes Map(this CodesDesc @this)
            => new Codes
            {
                Name = @this.Name,
                Items = _mapper
                .Map<ICollection<CurrencyCodes>>(@this.Items)
            };



        /// <summary>
        ///     Entity to gRPC mapping. 
        /// </summary>
        /// <param name="this"></param>
        /// <returns>Сущность контракта protobuf.</returns>
        public static CurrencyInfo Map(this CurrencyCodes @this)
            => _mapper.Map<CurrencyInfo>(@this);
    }
}
