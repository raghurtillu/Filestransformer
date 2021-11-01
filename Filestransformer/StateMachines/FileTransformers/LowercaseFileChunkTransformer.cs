using Filestransformer.StateMachines.FileTransformers.Events;
using Filestransformer.Support.Utils;
using Microsoft.PSharp;
using System;

namespace Filestransformer.StateMachines.FileTransformers
{
    /// <summary>
    /// Machine transforms a file chunk to lower case
    /// </summary>
    public partial class LowercaseFileChunkTransformer : Machine
    {
        public void HandleFileTransformChunkRequest()
        {
            var request = ReceivedEvent as eFileChunkTransformRequestEvent;
            var failureReason = string.Empty;

            try
            {
                string str = StreamUtils.GetFileChunkAsString(request.FileStream, request.FileChunkSizeToReadInBytes, request.FileEncoding);
                if (string.IsNullOrWhiteSpace(str))
                {
                    // file read completely
                    this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Success, null, failureReason));
                }
                else
                {
                    var transformedBytes = StreamUtils.GetBytesFromString(str.ToLower(), request.FileEncoding);
                    this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Success, transformedBytes, failureReason));
                }
            }
            catch (Exception ex)
            {
                failureReason = ex.Message;
                this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Failed, null, failureReason));
            }

            // send completion status to self and proceed to halt this machine
            this.Send(Id, new eFileChunkTransformCompletionEvent());
        }
    }
}
