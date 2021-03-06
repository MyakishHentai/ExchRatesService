using System;

namespace ExchRatesWCFService.Services.Interfaces
{
    public interface IBankService
    {
        string MarketName { get; }

        /// <summary>
        ///     Получение данных по кодам валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        T GetCodesInfoXml<T>() where T : class;

        /// <summary>
        ///     Получение данных котировок валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        T GetDailyInfoXml<T>(DateTime date) where T : class;
    }
}