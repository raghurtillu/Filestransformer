using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileTransformers.Events
{
    public class eFileTransformerEvent : Event
    {
        public ILogger Logger { get; }
        public string FileName { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public eFileTransformerEvent(ILogger logger, string fileName, string inputDirectory, string outputDirectory)
        {
            this.Logger = logger;
            this.FileName = fileName;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
        }
    }

    public class eTransformFileChunkRequest : Event
    {
        
    }
}
