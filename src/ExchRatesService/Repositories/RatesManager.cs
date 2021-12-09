using ExchRatesService.Models;
using ExchRatesService.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesService.Repositories
{
    public class RatesManager : IRatesManager
    {
        private bool _disposed = false;
        private readonly ExchRatesContext _context;
        private readonly ILogger<RatesManager> _logger;

        public RatesManager(ExchRatesContext context, ILogger<RatesManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Codes> Codes => _context.Codes.AsQueryable();
        public IQueryable<Quote> Quotes => _context.Quotes.AsQueryable();
        public IQueryable<CurrencyCodes> Currencies => _context.Currencies.AsQueryable();
        public IQueryable<QuoteCurrency> QuotesCurrencies => _context.QuotesCurrencies.AsQueryable();

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для котировок.
        /// </summary>
        /// <param name="codes">Котировки валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task AddCodesAsync(Codes codes)
        {
            try
            {
                _logger.LogInformation($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.Codes)}");
                if (_context.Currencies.Any())
                {
                    var idsCur = _context.Currencies.Select(x => x.Id).ToList();
                    var toUpdate = codes.Items.Where(x => idsCur.Contains(x.Id)).ToList();
                    await Task.Run(() => _context.Currencies.UpdateRange(toUpdate));
                }
                if (_context.Codes.Any())
                {
                    await Task.Run(() => _context.Codes.Update(codes));
                    return;
                }
                await _context.Codes.AddAsync(codes);
            }
            catch (Exception ex)
            {
                _logger.LogError($@"Возникло исключение при обновлении таблицы для 
                                {nameof(_context.Codes)}:{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для кодов валют.
        /// </summary>
        /// <param name="codes">Коды валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task AddQuotesAsync(ICollection<QuoteCurrency> quotesCur)
        {
            try
            {
                _logger.LogInformation($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.QuotesCurrencies)}");

                // Получение времени котировок.
                var times = _context.QuotesCurrencies.Select(x => x.Quote.Id).ToList();
                var toUpdate = quotesCur.Where(x => times.Contains(x.Quote.Id)).ToList();

                // Если есть уже котировки на этот период, то обновить их.
                if (toUpdate.Any())
                    await Task.Run(() => _context.QuotesCurrencies.UpdateRange(toUpdate));

                // Если все было обновлено
                if (toUpdate.Count == quotesCur.Count)
                    return;

                // Добавить новые значения и котировки.
                var toAdd = quotesCur.Where(x => !times.Contains(x.Quote.Id)).ToList();
                await _context.QuotesCurrencies.AddRangeAsync(toAdd);
            }
            catch (Exception ex)
            {
                _logger.LogError($@"Возникло исключение при обновлении таблицы для 
                                {nameof(_context.QuotesCurrencies)}:{ex.Message}", ex);
                throw;
            }
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
        public void Save() => _context.SaveChanges();

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
