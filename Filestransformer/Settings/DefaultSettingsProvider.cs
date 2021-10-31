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

            FileEncoding fileEncoding;
            Enum.TryParse(ConfigurationManager.AppSettings["FileEncoding"], true, out fileEncoding);

            settings = new Setting(fileGroups, maximumParallelFileTransformations, 
                inputDirectoryPath, outputDirectoryPath, fileChunkSizeToReadInBytes, fileEncoding);
        }

        public Setting GetSettings()
        {
            return settings;
        }
    }
}
