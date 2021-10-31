using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System;
using System.IO;


namespace Filestransformer.StateMachines.FileTransformers
{
    public abstract partial class FileTransformer : Machine
    {
        protected eFileTransformerEvent currentRequest;
        protected MachineId sender;
        protected ILogger logger;
        protected string fileName;
        protected string inputDirectory;
        protected string outputDirectory;
        protected DateTime timeOfRequest;

        protected virtual void InitializeFileTransformer()
        {
            currentRequest = ReceivedEvent as eFileTransformerEvent;
            timeOfRequest = DateTime.UtcNow;

            sender = currentRequest.Sender;
            logger = currentRequest.Logger;
            fileName = currentRequest.FileName;
            inputDirectory = currentRequest.InputDirectory;
            outputDirectory = currentRequest.OutputDirectory;
        }
    }
}
