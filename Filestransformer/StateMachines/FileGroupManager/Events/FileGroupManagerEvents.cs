using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileGroupManager.Events
{
    public class eFileGroupManagerConfig : Event
    {
        public ILogger Logger { get; }
        public int FileGroups { get; }
        public int MaximumParallelFileTransformations { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }

        // TODO add a setting for MaxOverallFileTransformations

        public eFileGroupManagerConfig(ILogger logger, int filegroups, int maximumParallelFileTransformations, 
            string inputDirectory, string outputDirectory)
        {
            this.Logger = logger;
            this.FileGroups = filegroups;
            this.MaximumParallelFileTransformations = maximumParallelFileTransformations;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
        }
    }
}
