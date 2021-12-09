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
        /// <summary>
        ///  Коды валют (рынок)
        /// </summary>
        IQueryable<Codes> Codes { get; }
        
        /// <summary>
        ///     Котировки (сведения справки)
        /// </summary>
        IQueryable<Quote> Quotes { get; }

        /// <summary>
        ///  Коды валют (содержимое)
        /// </summary>
        IQueryable<CurrencyCodes> Currencies { get; }

        /// <summary>
        ///     Значения котировок валют.
        /// </summary>
        IQueryable<QuoteCurrency> QuotesCurrencies { get; }

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для котировок.
        /// </summary>
        /// <param name="codes">Котировки валют.</param>
        /// <returns><see cref="Task"/></returns>
        Task AddCodesAsync(Codes codes);

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для кодов валют.
        /// </summary>
        /// <param name="codes">Коды валют.</param>
        /// <returns><see cref="Task"/></returns>
        Task AddQuotesAsync(ICollection<QuoteCurrency> quotes);

        /// <summary>
        ///     Сохранение контекста изменений.
        /// </summary>
        void Save();

        /// <summary>
        ///     Сохранение контекста изменений.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task SaveAsync();

    }
}
