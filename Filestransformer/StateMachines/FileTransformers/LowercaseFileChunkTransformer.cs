using Filestransformer.Settings;
using Filestransformer.StateMachines.FileTransformers.Events;
using Microsoft.PSharp;
using System;
using System.IO;
using System.Text;

namespace Filestransformer.StateMachines.FileTransformers
{
    public partial class LowercaseFileChunkTransformer : Machine
    {
        public void HandleFileTransformChunkRequest()
        {
            var request = ReceivedEvent as eFileChunkTransformRequestEvent;
            var failureReason = string.Empty;

            try
            {
                //string str = GetFileChunkAsString(request.FileStream, request.FileChunkSizeToReadInBytes, request.FileEncoding);
                string str = GetFileChunkAsString(request.FileStream, 4, request.FileEncoding);
                if (string.IsNullOrWhiteSpace(str))
                {
                    // file read completely
                    this.Send(request.Sender, new eFileChunkTransformResponseEvent(FileTransformationStatus.Success, null, failureReason));
                }
                else
                {
                    var transformedBytes = GetBytesFromString(str.ToLower(), request.FileEncoding);
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

        private string GetFileChunkAsString(FileStream fs, int chunkSize, FileEncoding encoding)
        {
            byte[] bytes = new byte[chunkSize];
            int chunkBytesRead = 0;

            while (chunkBytesRead < chunkSize)
            {
                if (fs.Position >= fs.Length) { break; }

                int bytesRead = fs.Read(bytes, chunkBytesRead, chunkSize - chunkBytesRead);
                if (bytesRead == 0) { break; }

                chunkBytesRead += bytesRead;
            }
            return GetStringFromBytes(bytes, chunkBytesRead, encoding);
        }

        private string GetStringFromBytes(byte[] buffer, int length, FileEncoding encoding)
        {
            switch (encoding)
            {
                case FileEncoding.ASCII:
                    return Encoding.ASCII.GetString(buffer, 0, length);
                case FileEncoding.UTF8:
                    return Encoding.UTF8.GetString(buffer, 0, length);
                case FileEncoding.UTF16:
                    return Encoding.Unicode.GetString(buffer, 0, length);
                case FileEncoding.UTF32:
                    return Encoding.UTF32.GetString(buffer, 0, length);
                default:
                    throw new InvalidOperationException($"Unexpected enum value for {nameof(FileEncoding)}: {encoding}");
            }
        }

        private byte[] GetBytesFromString(string str, FileEncoding encoding)
        {
            switch (encoding)
            {
                case FileEncoding.ASCII:
                    return Encoding.ASCII.GetBytes(str);
                case FileEncoding.UTF8:
                    return Encoding.UTF8.GetBytes(str);
                case FileEncoding.UTF16:
                    return Encoding.Unicode.GetBytes(str);
                case FileEncoding.UTF32:
                    return Encoding.UTF32.GetBytes(str);
                default:
                    throw new InvalidOperationException($"Unexpected enum value for {nameof(FileEncoding)}: {encoding}");
            }
        }
    }
}
