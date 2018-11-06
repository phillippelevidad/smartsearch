using System.IO;

namespace SmartSearch.LuceneNet
{
    static class IndexDirectoryHelper
    {
        public static string GetDirectoryPath(string baseDirectory, string domainName) => Path.Combine(baseDirectory, domainName);
    }
}
