using Filestransformer.StateMachines.FileTransformers.Events;
using System;
using System.IO;

namespace Filestransformer.StateMachines.FileTransformers
{
    public class LowercaseFileTransformer : FileTransformer
    {
        private FileStream inputFileStream;
        private FileStream outputFileStream;

        protected override void InitializeFileTransformer()
        {
            base.InitializeFileTransformer();

            try
            {
                inputFileStream = File.OpenRead(Path.Combine(inputDirectory, fileName));
                outputFileStream = File.OpenWrite(Path.Combine(outputDirectory, fileName));
                //outputFileStream = File.OpenWrite("c:\\users\\rags\\nonExistantFile.txt");
                //fileStream = File.OpenRead("c:\\users\\rags\\nonExistantFile.txt");
            }
            catch (Exception ex) 
            {
                status = FileTransformationStatus.Failed;
                SendFileTranformationResponse(FileTransformationStatus.Failed, fileName, ex.Message);
                                
                // send completion status to self and proceed to halt this machine
                this.Send(Id, new eFileTranformationCompletionEvent());
            }

            status = FileTransformationStatus.InProgress;
            //logger.WriteLine($"Initialized {nameof(LowercaseFileTransformer)} machine for {fileName} successfully.");
        }

        protected override void SendFileTransformationRequest()
        {
            var machine = this.CreateMachine(typeof(LowercaseFileChunkTransformer));
            this.Send(machine, new eFileChunkTransformRequestEvent(this.Id, inputFileStream));
        }

        protected override void HandleFileChunkResponse()
        {
            var response = ReceivedEvent as eFileChunkTransformResponseEvent;
            if (response.Status == FileTransformationStatus.Success)
            {
                status = FileTransformationStatus.Success;
                outputFileStream.Write(response.TransformedBytes, 0, response.TransformedBytes.Length);
                outputFileStream.Flush();
            }
            else if (response.Status == FileTransformationStatus.Failed)
            {
                status = FileTransformationStatus.Failed;
                failureReason = response.FailureReason;

                // stop processing file transformation
                // send completion status to self and proceed to halt this machine
                this.Send(Id, new eFileTranformationCompletionEvent());
            }
        }

        public override void HandleFileTransformationRequestCompleted()
        {
            SendFileTranformationResponse(status, fileName, failureReason);
            DisposeFileStreams();
        }

        protected override bool CompletedReadingInputFile() => 
            inputFileStream?.Position >= inputFileStream?.Length;
            
        private void DisposeFileStreams()
        {
            inputFileStream?.Close();
            outputFileStream?.Close();
        }

        private void SendFileTranformationResponse(FileTransformationStatus status, string fileName, string failureReason = "")
        {
            if (status == FileTransformationStatus.Success)
            {
                var completionTime = DateTime.UtcNow;
                var timeToComplete = completionTime.Subtract(timeOfRequest);
                this.Send(sender, new eFileTranformationResponseEvent(status, fileName, completionTime, timeToComplete, failureReason));
            }
            else if (status == FileTransformationStatus.Failed)
            {
                this.Send(sender, new eFileTranformationResponseEvent(status, fileName, null, null, failureReason));
            }
            else if (status == FileTransformationStatus.InProgress)
            {
                this.Send(sender, new eFileTranformationResponseEvent(status, fileName, null, null, string.Empty));
            }
            else
            {
                throw new InvalidOperationException($"Unexpected enum value for {nameof(FileTransformationStatus)}: {status}");
            }
        }
    }
}
