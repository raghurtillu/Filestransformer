using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileSystemWatcher
{
    /// <summary>
    /// Machine for monitor for new files in input directory <seealso cref="Setting.InputDirectoryPath"/>
    /// </summary>
    public abstract partial class FileSystemWatcherBase : Machine
    {
        /// <summary>
        /// Initializes machine
        /// </summary>
        protected abstract void InitializeFileSystemWatcher();

        protected abstract void Run();
    }
}
