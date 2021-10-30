using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileSystemWatcher.Events
{
    public class eFileSystemWatcherConfig : Event
    {
        public ILogger Logger { get; }

        public string DirectoryToWatch { get; }

        public MachineId FileGroupManager { get; }

        public eFileSystemWatcherConfig(ILogger logger, string directoryToWatch, MachineId fileGroupManager)
        {
            this.Logger = logger;
            this.DirectoryToWatch = directoryToWatch;
            this.FileGroupManager = fileGroupManager;
        }
    }
}
