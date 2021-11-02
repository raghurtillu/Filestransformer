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
            switch (logLevelContext)
            {
                case LogLevelContext.Verbose:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevelContext.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevelContext.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevelContext.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            
            Console.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")} {message}");
            Console.ResetColor();
        }
    }
}
