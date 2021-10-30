using Filestransformer.StateMachines.FileSystemWatcher.Events;
using Filestransformer.Support.Logger;
using Filestransformer.Support.Utils;
using System.Collections.Generic;
using System.IO;

namespace Filestransformer.StateMachines.FileSystemWatcher
{
    public partial class FileSystemWatcher : FileSystemWatcherBase
    {
        private string inputDirectory;
        private string directoryToWatch;
        private ILogger logger;
        private System.IO.FileSystemWatcher watcher;

        protected override void InitializeFileSystemWatcher()
        {
            var config = ReceivedEvent as eFileSystemWatcherConfig;
            inputDirectory = directoryToWatch = config.DirectoryToWatch;
            logger = config.Logger;

            logger.WriteLine($"Created {nameof(FileSystemWatcher)} machine to watch directory \"{directoryToWatch}\" ");
        }

        protected override void Run()
        {
            // get existing files in the directory if any
            var existingFiles = GetTextFilesInDirectory(inputDirectory);
            if (existingFiles?.Length > 0)
            {
                var filesInfo = GetGroupAndUnqualifiedFileNames(existingFiles);
                foreach (var fileInfo in filesInfo)
                {
                    logger.WriteLine($"Group: {fileInfo.Group}, filename: {fileInfo.FileName}, directory: {fileInfo.Directory}");
                }
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
            logger.WriteLine($"File created: {e.Name}");
        }

        private static string[] GetTextFilesInDirectory(string directory) => Directory.GetFiles(directory, "*.txt");

        private List<FileInfo> GetGroupAndUnqualifiedFileNames(string[] fileNames)
        {
            var fileInfoList = new List<FileInfo>();
            foreach (var fileName in fileNames)
            {
                string group, unqualifiedFileName;
                FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(Path.GetFileName(fileName),
                    out group, out unqualifiedFileName);
                fileInfoList.Add(new FileInfo(group, unqualifiedFileName, inputDirectory));
            }
            return fileInfoList;
        }
    }

    public class FileInfo
    {
        public string Group { get; }
        public string FileName { get; }
        public string Directory { get; }

        public FileInfo(string group, string fileName, string directory)
        {
            Group = group;
            FileName = fileName;
            Directory = directory;
        }
    }

}
