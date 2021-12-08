using ExchRatesService.Models;
using ExchRatesService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddCodesAsync(Codes codes)
        {
            try
            {
                _logger.LogInformation($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.Currencies)}");
                if (_context.Codes.Any())
                {
                    await Task.Run(() => _context.Codes.UpdateRange(codes));
                    return;
                }
                await _context.Codes.AddRangeAsync(codes);
            }
            catch (Exception ex)
            {
                _logger.LogError($@"Возникло исключение при обновлении таблицы для 
                                {nameof(_context.Currencies)}:{ex.Message}", ex);
                throw;
            }
        }

        public async Task AddQuotesAsync(ICollection<Quote> quotes)
        {
            try
            {
                _logger.LogInformation($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.Quotes)}");
                await _context.Quotes.AddRangeAsync(quotes);
            }
            catch (Exception ex)
            {
                _logger.LogError($@"Возникло исключение при обновлении таблицы для 
                                {nameof(_context.Quotes)}:{ex.Message}", ex);
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
