using ExchRatesService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesService.Repositories.Interfaces
{
    public interface IRatesManager : IDisposable
    {
        IQueryable<Codes> Codes { get; }
        IQueryable<Quote> Quotes { get; }
        IQueryable<CurrencyCodes> Currencies { get; }

        Task Save();
    }
}
