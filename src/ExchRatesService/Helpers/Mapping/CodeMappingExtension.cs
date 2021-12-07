using AutoMapper;
using CentralExchRateService;
using ExchRatesService.Models;
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
                cfg.CreateMap<CurrencyCodesDesc, CurrencyCodes>());
            // Настройка AutoMapper
            _mapper = new Mapper(config);
        }

        public static Codes Map(this CodesDesc codesDesc)
        {
            return new Codes
            {
                Name = codesDesc.Name,
                Items = _mapper
                        .Map<ICollection<CurrencyCodes>>(codesDesc.Items)
            };
        }
    }
}
