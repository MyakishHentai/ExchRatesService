using ExchRatesWCFService.Models.Entity;
using ExchRatesWCFService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Services
{
    public class DataBaseInfoService : IDataBaseService
    {
        private bool _disposed = false;
        private readonly ExchRatesContext _context;
        private readonly NLog.ILogger _logger;

        public DataBaseInfoService(NLog.ILogger logger)
        {
            _context = new ExchRatesContext();
            _logger = logger;
        }

        public IQueryable<Code> Codes => _context.Codes.AsQueryable();
        public IQueryable<Quote> Quotes => _context.Quotes.AsQueryable();
        public IQueryable<CodeQuote> QuotesCurrencies => _context.CodeQuotes.AsQueryable();

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для кодов валют.
        /// </summary>
        /// <param name="codes">Котировки валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task AddCodesAsync(ICollection<Code> codes)
        {
            try
            {
                _logger.Info($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.Codes)}");
                if (_context.Codes.Any())
                {
                    var idsCur = _context.Codes.Select(x => x.Id).ToList();
                    var toUpdate = codes.Where(x => idsCur.Contains(x.Id)).ToList();
                    await Task.Run(() =>
                    {
                        foreach (var item in toUpdate)
                        {
                            _context.Codes.AddOrUpdate(item);
                        }
                    });
                    return;
                }
                _context.Codes.AddRange(codes);
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
        /// <param name="codes">Коды валют.</param>
        /// <returns><see cref="Task"/></returns>
        public async Task AddQuotesAsync(ICollection<CodeQuote> quotesCur)
        {
            try
            {
                //_logger.Info($@"[{DateTime.Now}]:Обращение к таблице для {nameof(_context.CodeQuotes)}");

                //// Получение времени котировок.
                //var times = _context.CodeQuotes.Select(x => x.Quote.Date).ToList();
                //var toUpdate = quotesCur.Where(x => times.Contains(x.Quote.Date)).ToList();

                //// Если есть уже котировки на этот период, то обновить их.
                //if (toUpdate.Any())
                //{
                //    foreach (var item in _context.CodeQuotes.ToList())
                //    {
                //        item.Value = toUpdate.FirstOrDefault(x => x.CodeId == item.CodeId)?.Value ?? item.Value;
                //    }
                //    // Если все было обновлено
                //    if (toUpdate.Count == quotesCur.Count)
                //        return;
                //    await Task.Run(() =>
                //    {
                //        foreach (var item in toUpdate)
                //        {
                //            _context.CodeQuotes.AddOrUpdate(item);
                //        }
                //    });
                //}
                //if (_context.Codes.Any())
                //{
                //    foreach (var quote in quotesCur)
                //    {
                //        quote.Code = null;
                //    }
                //}
                //// Добавить новые значения и котировки.
                //var toAdd = quotesCur.Where(x => !times.Contains(x.Quote.Date)).ToList();

                //_context.CodeQuotes.AddRange(toAdd);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $@"Возникло исключение при обновлении таблицы для 
                                    {nameof(_context.Codes)}:{ex.Message}");
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
