using ExchRates.Common.Logger;
using Microsoft.Extensions.Logging;

namespace ExchRates.Common.Extensions
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}