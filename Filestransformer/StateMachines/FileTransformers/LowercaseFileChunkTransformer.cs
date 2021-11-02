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
                string chunkedString = StreamUtils.GetFileChunkAsString(request.FileStream, request.FileChunkSizeToReadInBytes, request.FileEncoding);
                if (!string.IsNullOrWhiteSpace(chunkedString))
                {
                    var transformedBytes = StreamUtils.GetBytesFromString(chunkedString.ToLower(), request.FileEncoding);
                    this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Success, 
                        transformedBytes, failureReason));
                }
                else
                {
                    // file read completely
                    this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Success, null, failureReason));
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
