namespace Filestransformer.Support.Utils
{
    public static class FullyQualifiedNameClient
    {
        public const char resourceDelimiter = '.';

        public static string[] SplitFullyQualfiedResource(string resource, int maxSplits)
        {
            char[] seperator = new char[] { resourceDelimiter };
            return resource.Split(seperator, maxSplits);
        }

        /// <summary>
        /// Splits a file name of the form 'group1.work1.txt' into 'group1' and 'work1.txt'
        /// </summary>
        /// <param name="fullyQualifiedFileName"></param>
        /// <param name="group"></param>
        /// <param name="unqualifiedFileName"></param>
        public static void GetGroupAndFileNameFromFullyQualifiedFileName(string fullyQualifiedFileName,
            out string group, out string unqualifiedFileName)
        {
            var splits = SplitFullyQualfiedResource(fullyQualifiedFileName, 2);
            group = splits[0];
            unqualifiedFileName = splits[1];
        }

        /// <summary>
        /// Gets fully qualified file name of the form 'group1.work1.txt' from groupName name 'group1' and unqualified file name 'work1.txt'
        /// </summary>
        /// <param name="group"></param>
        /// <param name="unqualifiedFileName"></param>
        /// <returns></returns>
        public static string GetFullyQualifiedFileName(string group, string unqualifiedFileName)
        {
            return group + resourceDelimiter + unqualifiedFileName;
        }
    }
}
