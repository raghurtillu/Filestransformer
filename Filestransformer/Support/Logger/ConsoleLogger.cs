using System;

namespace Filestransformer.Support.Logger
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        public void WriteLine(string message, LogLevelContext logLevelContext = LogLevelContext.Info)
        {
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")} {message}");
        }
    }
}
