using CommandLine;

namespace Filestransformer.Support.CommandLine
{
    /// <summary>
    /// Command line argument options
    /// </summary>
    public class Options
    {
        [Option('h', Required = false, HelpText = "Default help")]
        public string Help { get; set; } = "This is default help";

        [Option('p', Required = false, HelpText = "Maximum number of file transformations allowed per group in an epoch, e.g. 3")]
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
