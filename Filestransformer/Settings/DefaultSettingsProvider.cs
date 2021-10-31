using System;
using System.Configuration;

namespace Filestransformer.Settings
{
    public class DefaultSettingsProvider : ISettingsProvider
    {
        private readonly Setting settings;

        public DefaultSettingsProvider()
        {
            int fileGroups = Int32.Parse(ConfigurationManager.AppSettings["FileGroups"]);
            int maximumParallelFileTransformations = Int32.Parse(ConfigurationManager.AppSettings["MaximumParallelFileTransformations"]);
            string inputDirectoryPath = ConfigurationManager.AppSettings["InputDirectoryPath"];
            string outputDirectoryPath = ConfigurationManager.AppSettings["OutputDirectoryPath"];
            int fileChunkSizeToReadInBytes = Int32.Parse(ConfigurationManager.AppSettings["FileChunkSizeToReadInBytes"]);
            string fileEncodingType = ConfigurationManager.AppSettings["FileEncodingType"];

            settings = new Setting(fileGroups, maximumParallelFileTransformations, 
                inputDirectoryPath, outputDirectoryPath, fileChunkSizeToReadInBytes, fileEncodingType);
        }

        public Setting GetSettings()
        {
            return settings;
        }
    }
}
