using Filestransformer.Settings;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;
using System;
using System.IO;

namespace Filestransformer.StateMachines.FileTransformers.Events
{
    public class eFileTransformerEvent : Event
    {
        public MachineId Sender { get; }
        public ILogger Logger { get; }
        public string FileName { get; }
        public string InputDirectory { get; }
        public string OutputDirectory { get; }
        public int FileChunkSizeToReadInBytes { get; }
        public FileEncoding FileEncoding { get; }
        public eFileTransformerEvent(MachineId sender, ILogger logger, string fileName, string inputDirectory, string outputDirectory,
            int fileChunkSizeToReadInBytes, FileEncoding fileEncoding)
        {
            this.Sender = sender;            
            this.Logger = logger;
            this.FileName = fileName;
            this.InputDirectory = inputDirectory;
            this.OutputDirectory = outputDirectory;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
            this.FileEncoding = fileEncoding;
        }
    }

    public class eFileTranformationResponseEvent : Event
    {
        /// <summary>
        /// Status of transformation operation
        /// </summary>
        public FileTransformationStatus Status { get; }

        /// <summary>
        /// Transformation filename
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Time when the transformation completed, only valid when status is <seealso cref="FileTransformationStatus.Success"/>
        /// </summary>
        public DateTime? CompletionTime { get; }

        /// <summary>
        /// Time to complete the transformation, only valid when status is <seealso cref="FileTransformationStatus.Success"/>
        /// </summary>
        public TimeSpan? TimeToComplete { get; }

        /// <summary>
        /// Reason as to why the transformation failed, only valid when status is <seealso cref="FileTransformationStatus.Failed"/>
        /// </summary>
        public string FailureReason { get; }

        public eFileTranformationResponseEvent(FileTransformationStatus status, string fileName,
            DateTime? completionTime, TimeSpan? timeToComplete, string failureReason)
        {
            this.Status = status;
            this.FileName = fileName;
            this.CompletionTime = completionTime;
            this.TimeToComplete = timeToComplete;
            this.FailureReason = failureReason;
        }
    }

    public class eFileTranformationCompletionEvent : Event
    {
        
    }

    public class eFileChunkTransformRequestEvent : Event
    {
        public MachineId Sender { get; }

        public FileStream FileStream { get; }

        public int FileChunkSizeToReadInBytes { get; }

        public FileEncoding FileEncoding { get; }

        public eFileChunkTransformRequestEvent(MachineId sender, FileStream fileStream,
            int fileChunkSizeToReadInBytes = 1024, FileEncoding fileEncoding = FileEncoding.UTF8)
        {
            this.Sender = sender;
            this.FileStream = fileStream;
            this.FileChunkSizeToReadInBytes = fileChunkSizeToReadInBytes;
            this.FileEncoding = fileEncoding;
        }
    }

    public class eFileChunkTransformResponseEvent : Event
    {
        /// <summary>
        /// Status of file chunk transformation operation
        /// </summary>
        public FileTransformationStatus Status { get; }

        /// <summary>
        /// Transformed file chunk in bytes
        /// </summary>
        public byte[] TransformedBytes { get; }

        /// <summary>
        /// Reason as to why the transformation failed, only valid when status is <seealso cref="FileTransformationStatus.Failed"/>
        /// </summary>
        public string FailureReason { get; }

        public eFileChunkTransformResponseEvent(FileTransformationStatus status, byte[] transformedBytes, string failureReason)
        {
            this.Status = status;
            this.TransformedBytes = transformedBytes;
            this.FailureReason = failureReason;
        }
    }

    public class eFileChunkTransformCompletionEvent : Event
    {

    }
}
