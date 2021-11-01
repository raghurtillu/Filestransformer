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
            if (string.IsNullOrWhiteSpace(fullyQualifiedFileName))
            {
                group = string.Empty;
                unqualifiedFileName = string.Empty;
                return;
            }

            group = string.Empty;
            unqualifiedFileName = string.Empty;

            var splits = SplitFullyQualfiedResource(fullyQualifiedFileName, 2);
            if (splits.Length == 0) { return; }
            else if (splits.Length == 1) { unqualifiedFileName = splits[0]; return; }
            group = splits[0];
            unqualifiedFileName = splits[1];
            {
                var unqualifiedFileNameSplits = SplitFullyQualfiedResource(unqualifiedFileName, 2);
                if (unqualifiedFileNameSplits?.Length == 1)
                {
                    // unqualifiedFileName should be of the format, file1.txt, if the length of this split is 1
                    // then only the extension, txt, is parsed, which means the fullyQualifiedFileName was not in a 
                    // valid format group1.file1.txt in the first place

                    // empty out the group name and simply return unqualifiedFileName
                    unqualifiedFileName = GetFullyQualifiedFileName(group, unqualifiedFileNameSplits[0]);
                    group = string.Empty;
                }
            }
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
