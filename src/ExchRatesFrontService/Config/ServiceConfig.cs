using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRatesFrontService.Config
{
    /// <summary>
    ///     Тип конфигурации приложения.
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        ///     Адрес бэк-сервера.
        /// </summary>
        public string BACK_ADDR { get; set; }

        /// <summary>
        ///     Адрес кэш-сервиса.
        /// </summary>
        public string GAIA_ADDR { get; set; }

        /// <summary>
        ///     Безопасность.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        ///     Время хранение объектов в кеше (минуты)
        /// </summary>
        public int CacheTime { get; set; }
    }
}
