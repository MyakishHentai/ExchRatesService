using ExchRatesWCFService.Models.Entity;
using ExchRatesWCFService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Services
{
    public class DataBaseInfoService : IDataBaseService
    {
        private bool _disposed = false;
        private ExchRatesContext _context;
        private readonly NLog.ILogger _logger;

        public DataBaseInfoService(NLog.ILogger logger, ExchRatesContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IQueryable<Code> Codes => _context.Codes.AsQueryable();
        public IQueryable<Quote> Quotes => _context.Quotes.AsQueryable();
        public IQueryable<CodeQuote> CodeQuotes => _context.CodeQuotes.AsQueryable();


        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для кодов валют.
        /// </summary>
        /// <param name="newCodes">Коды валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateCodesAsync(IEnumerable<Code> newCodes)
        {
            try
            {
                _logger.Info($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.Codes)}");
                if (_context.Codes.Any())
                {
                    await Task.Run(() =>
                    {
                        foreach (var item in newCodes)
                        {
                            _context.Codes.AddOrUpdate(item);
                        }
                    });
                    await SaveAsync();
                    return;
                }
                _context.Codes.AddRange(newCodes);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Возникло исключение при обновлении таблицы для 
                                    {nameof(_context.Codes)}:{ex.Message}");
                throw;
            }
        }

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для котировок.
        /// </summary>
        /// <param name="newQuotes">Котировки валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateQuotesAsync(IEnumerable<CodeQuote> newQuotes)
        {
            try
            {
                _logger.Info($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.CodeQuotes)}");
                var dates = _context.CodeQuotes.Select(x => x.Quote.Date).ToList();
                var toUpdate = newQuotes
                    .Where(x=> dates.Contains(x.Quote.Date))
                    .ToList();
                // Если есть уже котировки на этот период, то обновить их.  
                if (toUpdate.Any())
                {
                    var upDates = toUpdate.Select(x => x.Quote.Date).ToList();
                    await Task.Run(() =>
                    {
                        var quotes = _context.CodeQuotes.Where(x => upDates.Contains(x.Quote.Date));
                        foreach (var quote in quotes)
                        {
                            quote.Value = toUpdate
                                .FirstOrDefault(x => x.Code.Id == quote.CodeId.Trim())?
                                .Value;
                        }
                    });
                    await SaveAsync();
                    return;
                }
                var quoteCurrent = newQuotes.FirstOrDefault().Quote;
                _context.Quotes.Add(quoteCurrent);
                await SaveAsync();

                // Все ли коды валют для котировок сущетствуют в базе
                var idCodes = _context.Codes.Select(x => x.Id.Trim()).ToList();
                var codesExist = newQuotes
                    .Where(x => idCodes.Contains(x.Code.Id.Trim()))
                    .ToList();
                foreach (var quote in codesExist)
                {
                    quote.Code = null;
                }
                await Task.Run(() =>
                {
                    foreach (var item in newQuotes)
                    {
                        item.QuoteId = quoteCurrent.Id;
                        item.Quote = null;
                        _context.CodeQuotes.Add(item);
                    }
                });
                await SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Возникло исключение при обновлении таблицы для 
                                    {nameof(_context.CodeQuotes)}:{ex.Message}");
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
