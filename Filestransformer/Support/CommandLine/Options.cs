﻿using CommandLine;

namespace Filestransformer.Support.CommandLine
{
    public class Options
    {
        [Option('h', Required = false, HelpText = "Default help")]
        public string Help { get; set; } = "This is default help";

        [Option('n', Required = false, HelpText = "Each file belongs one group, " +
            "each group can have arbitrary number of files associated with it")]
        public int FileGroups { get; set; }

        [Option('p', Required = false, HelpText = "Maximum number of file transformations allowed in an epoch")]
        public int MaximumParallelFileTransformations { get; set; }

        [Option('i', Required = false, HelpText = "Directory containing files to transform")]
        public string InputDirectoryPath { get; set; }

        [Option('o', Required = false, HelpText = "Directory contained transformed files")]
        public string OutputDirectoryPath { get; set; }
    }
}
