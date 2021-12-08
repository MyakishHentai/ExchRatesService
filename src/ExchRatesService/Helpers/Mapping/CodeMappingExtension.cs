using AutoMapper;
using CentralExchRateService;
using ExchRatesService.Models;
using ExchRatesSvc;
using System.Collections.Generic;

namespace ExchRatesService.Helpers.Mapping
{
    public static class CodeMappingExtension
    {
        private static readonly Mapper _mapper;
        static CodeMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<CurrencyCodesDesc, CurrencyCodes>();
                    cfg.CreateMap<CurrencyCodes, CurrencyInfo>();                    
                });
            // Настройка AutoMapper
            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     WCF to Entity
        /// </summary>
        /// <param name="codesDesc"></param>
        /// <returns></returns>
        public static Codes Map(this CodesDesc codesDesc)
        {
            return new Codes
            {
                Name = codesDesc.Name,
                Items = _mapper
                        .Map<ICollection<CurrencyCodes>>(codesDesc.Items)
            };
        }


        /// <summary>
        ///     Entity to gRPC.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static CurrencyInfo Map(this CurrencyCodes codes)
        {
            var currencyInfo = new CurrencyInfo();
            return _mapper.Map<CurrencyInfo>(codes);
        }
    }
}
