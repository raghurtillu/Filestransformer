using Filestransformer.Support.Utils;
using Xunit;

namespace Filestransformer.Tests
{
    public class FullyQualifiedNameClienTests
    {
        [Theory]
        [InlineData("", "", "")]
        [InlineData("txt", "", "txt")]
        [InlineData("nalanda.txt", "", "nalanda.txt")]
        [InlineData("group1.takshashila.txt", "group1", "takshashila.txt")]
        [InlineData("group0.group1.pataliputra.txt", "group0", "group1.pataliputra.txt")]
        public void GetGroupAndFileNameFromFullyQualifiedFileNameTests(string fullyQualifiedFileName, 
            string expectedGroupName, string expectedFileName)
        {
            string groupName, fileName;
            FullyQualifiedNameClient.GetGroupAndFileNameFromFullyQualifiedFileName(fullyQualifiedFileName,
                out groupName, out fileName);
            Assert.Equal(groupName, expectedGroupName);
            Assert.Equal(fileName, expectedFileName);
        }
    }
}
