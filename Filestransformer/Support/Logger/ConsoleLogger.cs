using System;

namespace Filestransformer.Support.Logger
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        public void WriteLine(string message, LogLevelContext logLevelContext = LogLevelContext.Verbose)
        {
            if (logLevelContext == LogLevelContext.Info)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (logLevelContext == LogLevelContext.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (logLevelContext == LogLevelContext.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")} {message}");

            Console.ResetColor();
        }
    }
}
