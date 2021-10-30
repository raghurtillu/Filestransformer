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
        public string FolderToWatch { get; }

        public ILogger Logger { get; }

        public eFileSystemWatcherConfig(string folderToWatch, ILogger logger)
        {
            this.FolderToWatch = folderToWatch;
            this.Logger = logger;
        }
    }
}
