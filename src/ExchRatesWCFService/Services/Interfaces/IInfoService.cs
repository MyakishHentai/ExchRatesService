using System;

namespace ExchRatesWCFService.Services.Interfaces
{
    public interface IInfoService
    {
        /// <summary>
        ///     Получение данных по кодам валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        T GetCodesInfoXML<T>() where T : class;

        /// <summary>
        ///     Получение данных котировок валют.
        /// </summary>
        /// <typeparam name="T">Тип десериализации.</typeparam>
        /// <returns>Сгенерированный тип.</returns>
        T GetDailyInfoXML<T>(DateTime date) where T : class;
    }
}
