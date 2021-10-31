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
        protected MachineId sender;
        protected ILogger logger;
        protected string fileName;
        protected string inputDirectory;
        protected string outputDirectory;
        
        protected DateTime timeOfRequest;
        protected FileTransformationStatus status;
        protected string failureReason;

        protected virtual void InitializeFileTransformer()
        {
            var config = ReceivedEvent as eFileTransformerEvent;
            timeOfRequest = DateTime.UtcNow;

            sender = config.Sender;
            logger = config.Logger;
            fileName = config.FileName;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;
        }

        protected abstract void SendFileTransformationRequest();

        protected abstract void HandleFileChunkResponse();

        protected abstract bool CompletedReadingInputFile();

        public abstract void HandleFileTransformationRequestCompleted();
    }
}
