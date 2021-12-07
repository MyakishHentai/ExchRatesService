using ExchRatesService.Models;
using ExchRatesService.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchRatesService.Repositories
{
    public class RatesManager : IRatesManager
    {
        private bool _disposed = false;
        readonly ExchRatesContext _context;

        public RatesManager(ExchRatesContext context)
        {
            _context = context;
        }

        public IQueryable<Codes> Codes => _context.Codes.AsQueryable();
        public IQueryable<Quote> Quotes => _context.Quotes.AsQueryable();
        public IQueryable<CurrencyCodes> Currencies => _context.Currencies.AsQueryable();

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

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
