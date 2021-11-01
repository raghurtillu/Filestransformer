using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filestransformer.Settings;
using Filestransformer.Support.Utils;
using Xunit;

namespace FilestransformerUnitTests
{
    public class StreamUtilsUnitTests
    {
        [Theory]
        [InlineData("blue", 0, FileEncoding.UTF8)]
        [InlineData("", 1, FileEncoding.UTF8)]
        [InlineData("H", 1, FileEncoding.ASCII)]
        [InlineData("H", 1, FileEncoding.UTF8)]
        [InlineData("H", 5, FileEncoding.UTF8)]
        [InlineData("Helloworld", 1, FileEncoding.UTF8)]
        [InlineData("Harappa and Mohenjodaro", 4, FileEncoding.UTF8)]
        [InlineData("I am a really long string, nalanda ujjain kosala magadha lumbini kanchipuram takshalashila", 32, FileEncoding.UTF8)]
        public void GetFileChunkAsStringTests(string str, int chunkSize, FileEncoding encoding)
        {
            var bytes = StreamUtils.GetBytesFromString(str, encoding);
            using (var ms = new MemoryStream(bytes))
            {
                if (chunkSize <= 0)
                {
                    Assert.True(string.IsNullOrWhiteSpace(StreamUtils.GetFileChunkAsString(ms, chunkSize, encoding)));
                    return;
                }

                int start = 0;
                while (start + chunkSize < str.Length)
                {
                    string expectedString = str.Substring(start, chunkSize);
                    start += chunkSize;

                    string actualString = StreamUtils.GetFileChunkAsString(ms, chunkSize, encoding);
                    Assert.NotNull(actualString);
                    Assert.Equal(expectedString.Length, actualString.Length);
                    Assert.Equal(expectedString, actualString);
                }
                if (start == str.Length)
                {
                    Assert.True(string.IsNullOrWhiteSpace(StreamUtils.GetFileChunkAsString(ms, chunkSize, encoding)));
                }
                else if (start < str.Length)
                {
                    string expectedString = str.Substring(start);

                    string actualString = StreamUtils.GetFileChunkAsString(ms, chunkSize, encoding);
                    Assert.NotNull(actualString);
                    Assert.Equal(expectedString.Length, actualString.Length);
                    Assert.Equal(expectedString, actualString);
                }
            }
        }
    }
}
