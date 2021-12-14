using ExchRatesWCFService.Mapping;
using ExchRatesWCFService.Models;
using ExchRatesWCFService.Services.Interfaces;
using NLog;
using Quartz;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Helpers
{
    public class CodesSheduler : IJob
    {
        private readonly IBankService _bankService;
        private readonly IDataBaseService _baseService;
        private readonly ILogger _logger;

        public CodesSheduler(
            ILogger logger,
            IBankService bank,
            IDataBaseService data)
        {
            _logger = logger;
            _bankService = bank;
            _baseService = data;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info($"[TaskScheduler] Вызов {nameof(Execute)}...");
                var date = DateTime.Now;
                using (_baseService)
                {
                    var quotesBase = _baseService.CodeQuotes
                        .Where(x => x.Quote.Date == date)
                        .AsNoTracking().ToList();

                    var quotesBank = _bankService.GetDailyInfoXml<QuoteBank>(date);
                    await _baseService
                        .UpdateQuotesAsync(quotesBank.Map());
                    _logger.Info($"[TaskScheduler] Обновлено {quotesBank.Valutes.Length}...");
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"[TaskScheduler] Ошибка при попытке обновления данных:\r\n{ex.Message}");
            }
        }
    }
}
