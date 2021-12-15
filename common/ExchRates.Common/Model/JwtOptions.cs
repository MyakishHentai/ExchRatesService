using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchRates.Common.Model
{
    public class JwtOptions
    {
        public const string Position = "JwtOptions";

        /// <summary>
        ///     Issuer для аутентификации.
        /// </summary>
        public string JwtIssuer { get; set; }

        /// <summary>
        ///     Key для аутентификации.
        /// </summary>
        public string JwtKey { get; set; }

        /// <summary>
        ///     Audience для аутентификации.
        /// </summary>
        public string Audience { get; set; }
    }
}
