using System;

namespace Filestransformer.Settings
{    
    /// <summary>
    /// Control settings for FileTransformer
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Number of file groups, e.g. for file group1.work1.txt, group1 is a group
        /// </summary>
        public int FileGroups { get; private set; }

        /// <summary>
        /// Number of files to transform from a group simultaneously
        /// </summary>
        public int MaxParallelFileTransformations { get; private set; }

        /// <summary>
        /// Input directory for files to transform
        /// </summary>
        public string InputDirectoryPath { get; private set; }

        /// <summary>
        /// Output directory where the transformed files will reside
        /// </summary>
        public string OutputDirectoryPath { get; private set; }

        /// <summary>
        /// Number of bytes to read from transformation file at a time
        /// </summary>
        public int FileChunkSizeToReadInBytes { get; private set; }

        /// <summary>
        /// Encoding type of transformed file <seealso cref="FileEncoding"/>
        /// </summary>
        public FileEncoding FileEncoding { get; private set; }

        public Setting(int fileGroups, int maxParallelFileTransformations, 
            string inputDirectoryPath, string outputDirectoryPath, int fileChunkSizeToReadInBytes, FileEncoding fileEncoding)
        {
            this.FileGroups = fileGroups;
            this.MaxParallelFileTransformations = maxParallelFileTransformations;
            this.InputDirectoryPath = inputDirectoryPath;
            this.OutputDirectoryPath = outputDirectoryPath;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
            this.FileEncoding = fileEncoding;
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

            if (otherSettings.FileEncoding != FileEncoding.Unknown)
            {
                this.FileEncoding = otherSettings.FileEncoding;
            }

            if (this.FileGroups <= 0) { throw new ArgumentException($"Invalid value '{this.FileGroups}' specified."); }
            if (this.MaxParallelFileTransformations <= 0) { throw new ArgumentException($"Invalid value '{this.MaxParallelFileTransformations}' specified."); }
            if (string.IsNullOrWhiteSpace(this.InputDirectoryPath)) { throw new ArgumentException($"Invalid value '{this.InputDirectoryPath}' specified."); }
            if (string.IsNullOrWhiteSpace(this.OutputDirectoryPath)) { throw new ArgumentException($"Invalid value '{this.OutputDirectoryPath}' specified."); }
            if (this.FileChunkSizeToReadInBytes <= 0) { throw new ArgumentException($"Invalid value '{this.FileChunkSizeToReadInBytes}' specified."); }
            if (this.FileEncoding == FileEncoding.Unknown) { throw new ArgumentException($"Invalid value '{this.FileEncoding}' specified."); }

            return this;
        }
    }

    public enum FileEncoding
    {
        UTF8,
        ASCII,
        UTF32,
        UTF16,
        Unknown,
    }
}
