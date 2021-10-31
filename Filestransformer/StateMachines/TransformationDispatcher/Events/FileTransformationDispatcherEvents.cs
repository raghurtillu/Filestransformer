using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.TransformationDispatcher.Events
{
    public class eFileTransformationDispatcherConfig : Event
    {
        public ILogger Logger { get; }
        public string Group { get; }
        public int MaximumParallelFileTransformations { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }

        public eFileTransformationDispatcherConfig(ILogger logger, string group, int maximumParallelFileTransformations,
            string inputDirectory, string outputDirectory)
        {
            this.Logger = logger;
            this.Group = group;
            this.MaximumParallelFileTransformations = maximumParallelFileTransformations;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
        }
    }
}
