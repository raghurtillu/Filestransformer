using Filestransformer.Support.CommandLine;

namespace Filestransformer.Settings
{
    public class ConsoleSettingsProvider : ISettingsProvider
    {
        private readonly Setting settings;

        public ConsoleSettingsProvider(Options op)
        {
            int fileGroups = op?.FileGroups ?? 0;
            int maximumParallelFileTransformations = op?.MaximumParallelFileTransformations ?? 0;
            string inputFolderPath = op?.InputFolderPath ?? "";
            string outputFolderPath = op?.OutputFolderPath ?? "";

            settings = new Setting(fileGroups, maximumParallelFileTransformations, inputFolderPath, outputFolderPath);
        }

        public Setting GetSettings()
        {
            return settings;
        }
    }
}
