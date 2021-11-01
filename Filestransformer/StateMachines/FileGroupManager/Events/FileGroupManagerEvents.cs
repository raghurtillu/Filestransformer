using Filestransformer.Settings;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileGroupManager.Events
{
    public class eFileGroupManagerConfig : Event
    {
        public ILogger Logger { get; }
        public int FileGroups { get; }
        public int MaximumParallelFileTransformations { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public int FileChunkSizeToReadInBytes { get; }
        public FileEncoding FileEncoding { get; }

        // TODO add a setting for MaxOverallFileTransformations in the system

        public eFileGroupManagerConfig(ILogger logger, int filegroups, int maximumParallelFileTransformations, 
            string inputDirectory, string outputDirectory, int fileChunkSizeToReadInBytes, FileEncoding fileEncoding)
        {
            this.Logger = logger;
            this.FileGroups = filegroups;
            this.MaximumParallelFileTransformations = maximumParallelFileTransformations;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
            this.FileEncoding = fileEncoding;
        }
    }
}
