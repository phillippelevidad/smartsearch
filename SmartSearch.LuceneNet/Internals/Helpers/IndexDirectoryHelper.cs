using System.IO;

namespace SmartSearch.LuceneNet.Internals.Helpers
{
    static class IndexDirectoryHelper
    {
        public static string GetDirectoryPath(string baseDirectory, string domainName) =>
            Path.Combine(baseDirectory, domainName);

        public static string GetFacetsDirectoryPath(string baseDirectory, string domainName) =>
            GetDirectoryPath(baseDirectory, $"{domainName}_facets");
    }
}
