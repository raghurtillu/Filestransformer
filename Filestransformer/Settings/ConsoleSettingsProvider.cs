﻿using Filestransformer.Support.CommandLine;
using System;

namespace Filestransformer.Settings
{
    /// <summary>
    /// Setting provider as provided from console
    /// </summary>
    public class ConsoleSettingsProvider : ISettingsProvider
    {
        private readonly Setting settings;

        public ConsoleSettingsProvider(Options op)
        {
            int maximumParallelFileTransformationsPerGroup = op?.MaximumParallelFileTransformationsPerGroup ?? 0;
            string inputDirectoryPath = op?.InputDirectoryPath ?? "";
            string outputDirectoryPath = op?.OutputDirectoryPath ?? "";
            int fileChunkSizeToReadInBytes = op?.FileChunkSizeToReadInBytes ?? 0;

            FileEncoding fileEncoding;
            Enum.TryParse(op?.FileEncoding ?? "unknown", true, out fileEncoding);

            settings = new Setting(maximumParallelFileTransformationsPerGroup, inputDirectoryPath, outputDirectoryPath, 
                fileChunkSizeToReadInBytes, fileEncoding);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public Setting GetSettings()
        {
            return settings;
        }
    }
}
