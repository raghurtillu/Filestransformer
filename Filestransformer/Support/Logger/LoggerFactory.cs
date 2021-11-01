using System;

namespace Filestransformer.Support.Logger
{
    internal enum LoggerType
    {
        Console = 1,
        // File = 2,
        // Null = 4
        // All = 8
    }

    internal static class LoggerFactory
    {
        private static readonly Lazy<ILogger> _lazyLogger = new Lazy<ILogger>(() => CreateLogger(LoggerType.Console));

        public static ILogger GetLogger() => _lazyLogger.Value;

        /// <summary>
        /// Creates logger (<seealso cref="ILogger"/> instance
        /// </summary>
        /// <param name="loggerType"><seealso cref="LoggerType"/></param>
        /// <returns>On success returns Logger instance <seealso cref="ILogger"/></returns>
        private static ILogger CreateLogger(LoggerType loggerType = LoggerType.Console)
        {
            if (loggerType == LoggerType.Console)
            {
                return new ConsoleLogger();
            }
            else
            {
                throw new NotImplementedException("Only supporting console logger currently.");
            }
        }
    }
}
