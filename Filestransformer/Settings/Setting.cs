using System;

namespace Filestransformer.Settings
{    
    /// <summary>
    /// Control settings for FileTransformer
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Number of files to transform from a group simultaneously
        /// </summary>
        public int MaxParallelFileTransformationsPerGroup { get; private set; }

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

        public Setting(int maxParallelFileTransformationsPerGroup, string inputDirectoryPath, string outputDirectoryPath, 
            int fileChunkSizeToReadInBytes, FileEncoding fileEncoding)
        {
            this.MaxParallelFileTransformationsPerGroup = maxParallelFileTransformationsPerGroup;
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
            if (otherSettings.MaxParallelFileTransformationsPerGroup != 0)
            {
                this.MaxParallelFileTransformationsPerGroup = otherSettings.MaxParallelFileTransformationsPerGroup;
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
            return this;
        }
    }

    public static class SettingExtensions
    {
        public static Setting Validate(this Setting settings)
        {
            if (settings.MaxParallelFileTransformationsPerGroup <= 0)
            { 
                throw new ArgumentException($"Invalid value '{settings.MaxParallelFileTransformationsPerGroup}' for " +
                    $"{nameof(settings.MaxParallelFileTransformationsPerGroup)} specified.");
            }
            if (string.IsNullOrWhiteSpace(settings.InputDirectoryPath))
            { 
                throw new ArgumentException($"Invalid value '{settings.InputDirectoryPath}' for " +
                    $"{nameof(settings.InputDirectoryPath)} specified.");
            }
            if (string.IsNullOrWhiteSpace(settings.InputDirectoryPath))
            { 
                throw new ArgumentException($"Invalid value '{settings.InputDirectoryPath}' for " +
                    $"{nameof(settings.InputDirectoryPath)} specified.");
            }
            if (string.IsNullOrWhiteSpace(settings.OutputDirectoryPath))
            { 
                throw new ArgumentException($"Invalid value '{settings.OutputDirectoryPath}' for " +
                    $"{nameof(settings.OutputDirectoryPath)} specified.");
            }
            if (settings.FileChunkSizeToReadInBytes <= 0)
            { 
                throw new ArgumentException($"Invalid value '{settings.FileChunkSizeToReadInBytes}' for " +
                    $"{nameof(settings.FileChunkSizeToReadInBytes)} specified.");
            }
            if (settings.FileEncoding == FileEncoding.Unknown)
            { 
                throw new ArgumentException($"Invalid value '{settings.FileEncoding}' for " +
                    $"{nameof(settings.FileEncoding)} specified.");
            }
            return settings;
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
