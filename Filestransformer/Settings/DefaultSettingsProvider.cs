using Filestransformer.Settings;
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
            string inputFolderPath = ConfigurationManager.AppSettings["InputFolderPath"];
            string outputFolderPath = ConfigurationManager.AppSettings["OutputFolderPath"];

            settings = new Setting(fileGroups, maximumParallelFileTransformations, inputFolderPath, outputFolderPath);
        }

        public Setting GetSettings()
        {
            return settings;
        }
    }
}
