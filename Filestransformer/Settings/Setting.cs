using System;

namespace Filestransformer.Settings
{
    public class Setting
    {
        public int FileGroups { get; private set; }

        public int MaxParallelFileTransformations { get; private set; }

        public string InputDirectoryPath { get; private set; }

        public string OutputDirectoryPath { get; private set; }

        public int FileChunkSizeToReadInBytes { get; private set; }

        public Setting(int fileGroups, int maxParallelFileTransformations, 
            string inputDirectoryPath, string outputDirectoryPath, int fileChunkSizeToReadInBytes)
        {
            this.FileGroups = fileGroups;
            this.MaxParallelFileTransformations = maxParallelFileTransformations;
            this.InputDirectoryPath = inputDirectoryPath;
            this.OutputDirectoryPath = outputDirectoryPath;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
        }

        /// <summary>
        /// Merges settings from <seealso cref="Settings"/> if current instance members are not set
        /// </summary>
        /// <param name="otherSettings"></param>
        public Setting MergeFrom(Setting otherSettings)
        {
            if (otherSettings.FileGroups != 0)
            {
                this.FileGroups = otherSettings.FileGroups;
            }
            
            if (otherSettings.MaxParallelFileTransformations != 0)
            {
                this.MaxParallelFileTransformations = otherSettings.MaxParallelFileTransformations;
            }

            if (!string.IsNullOrWhiteSpace(otherSettings.InputDirectoryPath))
            {
                this.InputDirectoryPath = otherSettings.InputDirectoryPath;
            }
            this.InputDirectoryPath = Environment.ExpandEnvironmentVariables(this.InputDirectoryPath);

            if (!string.IsNullOrWhiteSpace(otherSettings.OutputDirectoryPath))
            {
                this.OutputDirectoryPath = otherSettings.OutputDirectoryPath;
            }
            this.OutputDirectoryPath = Environment.ExpandEnvironmentVariables(this.OutputDirectoryPath);

            if (otherSettings.FileChunkSizeToReadInBytes != 0)
            {
                this.FileChunkSizeToReadInBytes = otherSettings.FileChunkSizeToReadInBytes;
            }
            return this;
        }
    }
}
