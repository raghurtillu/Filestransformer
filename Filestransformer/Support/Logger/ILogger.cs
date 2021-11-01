using System;

namespace Filestransformer.Support.Logger
{
    /// <summary>
    /// Log level 
    /// </summary>
    [Flags]
    public enum LogLevelContext
    {
        Verbose = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    /// <summary>
    /// Logger interface
    /// </summary>
    public interface ILogger
    {
        void WriteLine(string message, LogLevelContext logLevelContext = LogLevelContext.Verbose);
    }
}
