using System;
using Microsoft.Extensions.Logging;

namespace KJ1012.Core.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInfo(this ILogger logger, string message, params object[] args)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            logger.LogInformation(message, args);
            Console.WriteLine($"[{DateTime.Now}] {message}");
        }
    }
}
