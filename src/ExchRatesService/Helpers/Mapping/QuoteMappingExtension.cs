using AutoMapper;
using CentralExchRateService;
using ExchRatesService.Models;
using ExchRatesSvc;

namespace ExchRatesService.Helpers.Mapping
{
    public static class QuoteMappingExtension
    {
        private static readonly Mapper _mapper;
        static QuoteMappingExtension()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    // Entity
                    cfg.CreateMap<QuoteDesc, QuoteInfo>();
                    // gRPC
                    cfg.CreateMap<CurrencyCodesDesc, Quote>();
                });

            _mapper = new Mapper(config);
        }

        /// <summary>
        ///     gRPC.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static QuoteInfo Map(this CurrencyCodesDesc @this)
        {
            return _mapper.Map<QuoteInfo>(@this);
        }
    }
}
