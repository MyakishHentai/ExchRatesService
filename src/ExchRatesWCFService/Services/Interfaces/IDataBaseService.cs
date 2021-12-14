using ExchRatesWCFService.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesWCFService.Services.Interfaces
{
    public interface IDataBaseService : IDisposable
    {
        /// <summary>
        ///  Коды валют (рынок)
        /// </summary>
        IQueryable<Code> Codes { get; }
        
        /// <summary>
        ///     Котировки (сведения справки)
        /// </summary>
        IQueryable<Quote> Quotes { get; }

        /// <summary>
        ///     Значения котировок валют.
        /// </summary>
        IQueryable<CodeQuote> CodeQuotes { get; }

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для кодов валют.
        /// </summary>
        /// <param name="codes">Котировки валют.</param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateCodesAsync(IEnumerable<Code> codes);

        /// <summary>
        ///     Обеспечивает корректное добавление/обновление сущностей для котировок.
        /// </summary>
        /// <param name="codes">Коды валют.</param>
        /// <returns><see cref="Task"/></returns>
        Task UpdateQuotesAsync(IEnumerable<CodeQuote> quotes);

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
