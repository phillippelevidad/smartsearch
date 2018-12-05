using Lucene.Net.Spatial.Prefix;
using Lucene.Net.Spatial.Prefix.Tree;
using Spatial4n.Core.Context;

namespace SmartSearch.LuceneNet.Internals
{
    class SpatialFactory
    {
        public const int MaxLevels = 11;

        public static SpatialContext CreateSpatialContext() => SpatialContext.GEO;

        public static GeohashPrefixTree CreatePrefixTree() => new GeohashPrefixTree(CreateSpatialContext(), MaxLevels);

        public static RecursivePrefixTreeStrategy CreatePrefixTreeStrategy(string fieldName) => new RecursivePrefixTreeStrategy(CreatePrefixTree(), fieldName);
    }
}
