using Filestransformer.Settings;
using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System;


namespace Filestransformer.StateMachines.FileTransformers
{
    /// <summary>
    /// File transformation machine
    /// </summary>
    public abstract partial class FileTransformer : Machine
    {
        protected MachineId sender;
        protected ILogger logger;
        protected string fileName;
        protected string inputDirectory;
        protected string outputDirectory;
        protected int fileChunkSizeToReadInBytes;
        protected FileEncoding fileEncoding;

        protected DateTime timeOfRequest;
        protected FileTransformationStatus status;
        protected string failureReason;

        /// <summary>
        /// Initializes machine
        /// </summary>
        protected virtual void InitializeFileTransformer()
        {
            var config = ReceivedEvent as eFileTransformerEvent;
            timeOfRequest = DateTime.UtcNow;

            sender = config.Sender;
            logger = config.Logger;
            fileName = config.FileName;
            inputDirectory = config.InputDirectory;
            outputDirectory = config.OutputDirectory;
            fileChunkSizeToReadInBytes = config.FileChunkSizeToReadInBytes;
            fileEncoding = config.FileEncoding;
        }

        /// <summary>
        /// Dispatches file transformation request to a downstream machine to transform a chunk of file contents
        /// <seealso cref="eFileChunkTransformRequestEvent"/>
        /// </summary>
        protected abstract void SendFileTransformationRequest();

        /// <summary>
        /// Handler for file chunk transformation response event
        /// <seealso cref="eFileChunkTransformResponseEvent"/>
        /// </summary>
        protected abstract void HandleFileChunkResponse();

        /// <summary>
        /// Determines if the transformation input file has been fully transformed
        /// </summary>
        /// <returns></returns>
        protected abstract bool CompletedReadingInputFile();

        /// <summary>
        /// Handler for file transformation completion event, for clean up of resources etc.,
        /// <seealso cref="eFileChunkTransformCompletionEvent"/>
        /// </summary>
        public abstract void HandleFileTransformationRequestCompleted();
    }
}
