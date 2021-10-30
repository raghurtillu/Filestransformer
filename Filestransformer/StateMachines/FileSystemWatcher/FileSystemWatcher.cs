using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.Support.Logger;
using System.IO;

namespace Filestransformer.StateMachines.FileSystemWatcher
{
    public partial class FileSystemWatcher : FileSystemWatcherBase
    {
        private string folderToWatch;
        private ILogger logger;
        private System.IO.FileSystemWatcher watcher;

        protected override void InitializeFileSystemWatcher()
        {
            var config = ReceivedEvent as eFileSystemWatcherConfig;
            folderToWatch = config.FolderToWatch;
            logger = config.Logger;

            logger.WriteLine($"created {nameof(FileSystemWatcher)} machine to watch folder \"{folderToWatch}\" ");
        }

        protected override void Run()
        {
            // scan for existing files in the directory
            var files = Directory.GetFiles(folderToWatch, "*.txt");
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                logger.WriteLine($"Found file {fileName}");
            }

            watcher = new System.IO.FileSystemWatcher(folderToWatch);
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            // only monitor for text files
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;

            watcher.Created += OnCreated;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            logger.WriteLine($"File created: {e.Name}");
        }
    }
}
