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
        public MachineId Sender { get; }
        public ILogger Logger { get; }
        public string FileName { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public eFileTransformerEvent(MachineId sender, ILogger logger, string fileName, string inputDirectory, string outputDirectory)
        {
            this.Sender = sender;            
            this.Logger = logger;
            this.FileName = fileName;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
        }
    }

    public class eFileTransformChunkRequestEvent : Event
    {

    }

    public class eFileTranformationResponseEvent : Event
    {
        /// <summary>
        /// Status of transformation operation
        /// </summary>
        public FileTransformationStatus Status { get; set; }
        
        /// <summary>
        /// Time when the transformation completed, only valid when status is <seealso cref="FileTransformationStatus.Success"/>
        /// </summary>
        public DateTime CompletionTime { get; set; }

        /// <summary>
        /// Time to complete the transformation, only valid when status is <seealso cref="FileTransformationStatus.Success"/>
        /// </summary>
        public TimeSpan TimeToComplete { get; set; }

        /// <summary>
        /// Reason as to why the transformation failed, only valid when status is <seealso cref="FileTransformationStatus.Failed"/>
        /// </summary>
        public string FailureReason { get; set; }
    }
}
