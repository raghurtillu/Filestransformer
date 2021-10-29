namespace Filestransformer
{
    public class Settings
    {
        private static readonly Settings s_instance = new Settings();
        public static Settings Instance => s_instance;

        private Settings()
        {

        }

        public int FileGroups { get; private set; }

        public int MaxParallelFileTransformations { get; private set; }

        public void Initialize(int fileGroups, int maxParallelFileTransformations)
        {
            FileGroups = fileGroups;
            MaxParallelFileTransformations = maxParallelFileTransformations;
        }
    }
}
