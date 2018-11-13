using System.IO;

namespace SmartSearch.LuceneNet.Internals.Helpers
{
    static class IndexDirectoryHelper
    {
        public static string GetDirectoryPath(string baseDirectory) =>
            Path.Combine(baseDirectory, "mainindex");

        public static string GetFacetsDirectoryPath(string baseDirectory) =>
            Path.Combine(baseDirectory, "facets");
    }
}
