using Filestransformer.Settings;
using System;
using System.IO;
using System.Text;

namespace Filestransformer.Support.Utils
{
    /// <summary>
    /// Stream utilities around reading chunks of read stream, conversting to string, bytes etc
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Reads chunksize size content from stream as a string
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="chunkSize"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetFileChunkAsString(Stream stream, int chunkSize, FileEncoding encoding)
        {
            if (stream == null || chunkSize <= 0 || encoding == FileEncoding.Unknown)
            {
                return string.Empty;
            }

            byte[] bytes = new byte[chunkSize];
            int chunkBytesRead = 0;

            while (chunkBytesRead < chunkSize)
            {
                if (stream.Position >= stream.Length) { break; }

                int bytesRead = stream.Read(bytes, chunkBytesRead, chunkSize - chunkBytesRead);
                if (bytesRead == 0) { break; }

                chunkBytesRead += bytesRead;
            }
            return GetStringFromBytes(bytes, chunkBytesRead, encoding);
        }

        public static byte[] GetBytesFromString(string str, FileEncoding encoding)
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

        public static string GetStringFromBytes(byte[] buffer, int length, FileEncoding encoding)
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

    }
}
