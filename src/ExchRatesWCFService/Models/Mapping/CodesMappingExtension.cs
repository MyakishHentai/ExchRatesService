using AutoMapper;
using ExchRatesWCFService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Models.Mapping
{
    public static class CodesMappingExtension
    {
        private static readonly Mapper _mapper;
        static CodesMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg => 
                cfg.CreateMap<CurrencyCodesDesc, CurrencyCodesEntity>());
            // Настройка AutoMapper
            _mapper = new Mapper(config);
        }

        public static CodesEntity Map(this CodesDesc codesDesc)
        {
            return new CodesEntity
            {
                Name = codesDesc.Name,
                Items = _mapper
                        .Map<ICollection<CurrencyCodesEntity>>(codesDesc.Items)
        };
        }
    }
}
