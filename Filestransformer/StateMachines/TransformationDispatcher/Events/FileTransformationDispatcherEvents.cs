using Filestransformer.Settings;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.TransformationDispatcher.Events
{
    public class eFileTransformationDispatcherConfig : Event
    {
        public ILogger Logger { get; }
        public string Group { get; }
        public int MaximumParallelFileTransformationsPerGroup { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public int FileChunkSizeToReadInBytes { get; }
        public FileEncoding FileEncoding { get; }

        public eFileTransformationDispatcherConfig(ILogger logger, string group, int maximumParallelFileTransformations,
            string inputDirectory, string outputDirectory, int fileChunkSizeToReadInBytes, FileEncoding fileEncoding)
        {
            this.Logger = logger;
            this.Group = group;
            this.MaximumParallelFileTransformationsPerGroup = maximumParallelFileTransformations;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
            this.FileEncoding = fileEncoding;
        }
    }
}
