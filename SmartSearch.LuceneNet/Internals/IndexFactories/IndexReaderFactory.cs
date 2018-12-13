using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;

namespace SmartSearch.LuceneNet.Internals.IndexFactories
{
    internal static class IndexReaderFactory
    {
        public static TaxonomyReader CreateFacetsReader(IndexContextWrapper contextWrapper)
        {
            return new DirectoryTaxonomyReader(contextWrapper.FacetsDirectory);
        }

        public static DirectoryReader CreateIndexReader(IndexContextWrapper contextWrapper)
        {
            return DirectoryReader.Open(contextWrapper.IndexDirectory);
        }
    }
}