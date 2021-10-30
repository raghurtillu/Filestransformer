using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileSystemWatcher.Events
{
    public class eFileSystemWatcherConfig : Event
    {
        public string DirectoryToWatch { get; }

        public ILogger Logger { get; }

        public eFileSystemWatcherConfig(string directoryToWatch, ILogger logger)
        {
            this.DirectoryToWatch = directoryToWatch;
            this.Logger = logger;
        }
    }
}
