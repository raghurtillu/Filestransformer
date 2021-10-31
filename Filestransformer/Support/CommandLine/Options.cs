using CommandLine;

namespace Filestransformer.Support.CommandLine
{
    public class Options
    {
        [Option('h', Required = false, HelpText = "Default help")]
        public string Help { get; set; } = "This is default help";

        [Option('n', Required = false, HelpText = "Each file belongs one group, " +
            "each group can have arbitrary number of files associated with it, e.g. 3")]
        public int FileGroups { get; set; }

        [Option('p', Required = false, HelpText = "Maximum number of file transformations allowed in an epoch, e.g. 3")]
        public int MaximumParallelFileTransformations { get; set; }

        [Option('i', Required = false, HelpText = "Directory containing files to transform, e.g. c:\\users\\rags\\input")]
        public string InputDirectoryPath { get; set; }

        [Option('o', Required = false, HelpText = "Directory contained transformed files, e.g. c:\\users\\rags\\output")]
        public string OutputDirectoryPath { get; set; }

        [Option('f', Required = false, HelpText = "File chunk size to read in bytes, e.g. 1024")]
        public int FileChunkSizeToReadInBytes { get; set; }

        [Option('e', Required = false, HelpText = "File encoding, e.g. UTF8")]
        public string FileEncoding { get; set; }
    }
}
