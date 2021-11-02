using System;
using System.Configuration;

namespace Filestransformer.Settings
{
    /// <summary>
    /// Setting provider backed by App.config, is the default settings if not overriden from console
    /// </summary>
    public class DefaultSettingsProvider : ISettingsProvider
    {
        private readonly Setting settings;

        public DefaultSettingsProvider()
        {
            int maximumParallelFileTransformations = Int32.Parse(ConfigurationManager.AppSettings["MaximumParallelFileTransformations"]);
            string inputDirectoryPath = ConfigurationManager.AppSettings["InputDirectoryPath"];
            string outputDirectoryPath = ConfigurationManager.AppSettings["OutputDirectoryPath"];
            int fileChunkSizeToReadInBytes = Int32.Parse(ConfigurationManager.AppSettings["FileChunkSizeToReadInBytes"]);

            FileEncoding fileEncoding;
            Enum.TryParse(ConfigurationManager.AppSettings["FileEncoding"], true, out fileEncoding);

            settings = new Setting(maximumParallelFileTransformations, inputDirectoryPath, outputDirectoryPath, 
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
