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
        public string BackAddress { get; set; }

        /// <summary>
        ///     Адрес Reddis or e.t.c.
        /// </summary>
        public string CacheAddress { get; set; }

        /// <summary>
        ///     Путь файла для кэш-сервиса.
        /// </summary>
        public bool IsMemoryCache { get; set; } = true;

        /// <summary>
        ///     Настройка для gRPC.
        /// </summary>
        public bool IsSecure { get; set; }

        /// <summary>
        ///     Нужно ли дополнительно вести лог в файл.
        /// </summary>
        public bool IsFileLog { get; set; }

        #region Static => Общедоступные значения:

        /// <summary>
        ///     Время хранение объектов в кэше (минуты)
        /// </summary>
        public static int CacheTime { get; set; } = 5;

        /// <summary>
        ///     JwtKey для аутентификации.
        /// </summary>
        public static string JwtKey { get; set; }

        /// <summary>
        ///     JwtIssuer для аутентификации.
        /// </summary>
        public static string JwtIssuer { get; set; }

        /// <summary>
        ///     Audience для аутентификации.
        /// </summary>
        public static string Audience { get; set; }

        #endregion
    }
}