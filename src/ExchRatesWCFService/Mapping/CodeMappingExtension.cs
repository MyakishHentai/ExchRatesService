using AutoMapper;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Models.Entity;
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
                    cfg.CreateMap<Code, CodeBank>()
                    .ReverseMap();
                });
            _mapper = new Mapper(config);
        }

        public static ICollection<CodeBank> Map(this ICollection<Code> @this)
            => _mapper.Map<ICollection<Code>, ICollection<CodeBank>>(@this);

        public static ICollection<Code> Map(this ICollection<CodeBank> @this)
            => _mapper.Map<ICollection<CodeBank>, ICollection<Code>>(@this);
    }
}
