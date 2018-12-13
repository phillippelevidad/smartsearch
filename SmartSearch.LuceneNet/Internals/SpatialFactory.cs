using Lucene.Net.Spatial.Prefix;
using Lucene.Net.Spatial.Prefix.Tree;
using Spatial4n.Core.Context;

namespace SmartSearch.LuceneNet.Internals
{
    internal class SpatialFactory
    {
        public const int MaxLevels = 11;

        public static GeohashPrefixTree CreatePrefixTree() => new GeohashPrefixTree(CreateSpatialContext(), MaxLevels);

        public static RecursivePrefixTreeStrategy CreatePrefixTreeStrategy(string fieldName) => new RecursivePrefixTreeStrategy(CreatePrefixTree(), fieldName);

        public static SpatialContext CreateSpatialContext() => SpatialContext.GEO;
    }
}