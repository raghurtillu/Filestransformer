using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.StateMachines.FileGroupManager.Events;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using System.Collections.Generic;
using System.IO;
using Microsoft.PSharp;
using System.Linq;

namespace Filestransformer.StateMachines.FileSystemWatcher
{
    public class FileSystemWatcher : FileSystemWatcherBase
    {
        private ILogger logger;
        private string inputDirectory;
        private string directoryToWatch;
        private System.IO.FileSystemWatcher watcher;
        private MachineId fileGroupManager;

        protected override void InitializeFileSystemWatcher()
        {
            var config = ReceivedEvent as eFileSystemWatcherConfig;
            logger = config.Logger;
            inputDirectory = directoryToWatch = config.DirectoryToWatch;
            fileGroupManager = config.FileGroupManager;

            logger.WriteLine($"Initialized {nameof(FileSystemWatcher)} machine to watch directory \"{directoryToWatch}\" ");
        }

        protected override void Run()
        {
            // get existing files in the directory if any
            var existingFiles = Directory.GetFiles(inputDirectory, "*.txt").ToList();
            if (existingFiles?.Count > 0)
            {
                logger.WriteLine($"Found '{existingFiles.Count}' existing files in '{inputDirectory}' to transform");

                // strip out path from the filenames
                existingFiles = existingFiles.Select(x => Path.GetFileName(x)).ToList();

                this.Send(fileGroupManager, new eAddFilesToTransform(existingFiles));
            }

            // watch for new files in the directory
            watcher = new System.IO.FileSystemWatcher(directoryToWatch);
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Filter = "*.txt";   // monitor for text files only
            watcher.IncludeSubdirectories = false;  // only in current directory, not child directories
            watcher.EnableRaisingEvents = true;

            watcher.Created += OnCreated;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            this.Send(fileGroupManager, new eAddFileToTransform(e.Name));
        }
    }
}
