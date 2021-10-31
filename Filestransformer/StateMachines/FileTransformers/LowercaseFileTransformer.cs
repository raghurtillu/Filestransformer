using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.StateMachines.CommonEvents;
using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.Support.Logger;
using Microsoft.PSharp;

namespace Filestransformer.StateMachines.FileTransformers
{
    public class LowercaseFileTransformer : FileTransformer
    {
        private FileStream fileStream;

        protected override void InitializeFileTransformer()
        {
            base.InitializeFileTransformer();

            try
            {
                fileStream = File.OpenRead("c:\\users\\rags\\nonExistantFile.txt");
            }
            catch (Exception ex)
            {
                SendFileTranformationResponse(FileTransformationStatus.Failed, fileName, ex.Message);
                return;
            }

            logger.WriteLine($"Initialized {nameof(LowercaseFileTransformer)} machine for {fileName} successfully.");
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
