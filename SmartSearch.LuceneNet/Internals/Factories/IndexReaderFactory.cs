using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;
using SmartSearch.LuceneNet.Internals.Helpers;

namespace SmartSearch.LuceneNet.Internals.Factories
{
    static class IndexReaderFactory
    {
        public static TaxonomyReader CreateFacetsReader(InternalSearchDomain domain, LuceneIndexOptions options)
        {
            if (options.IndexInMemory)
            {
                return new DirectoryTaxonomyReader(new RAMDirectory());
            }
            else
            {
                var path = IndexDirectoryHelper.GetFacetsDirectoryPath(options.IndexDirectory, domain.Name);
                return new DirectoryTaxonomyReader(FSDirectory.Open(path));
            }
        }

        public static DirectoryReader CreateIndexReader(InternalSearchDomain domain, LuceneIndexOptions options)
        {
            if (options.IndexInMemory)
            {
                using (var dir = FSDirectory.Open(options.IndexDirectory))
                    return DirectoryReader.Open(new RAMDirectory(dir, IOContext.DEFAULT));
            }
            else
            {
                var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);
                return DirectoryReader.Open(FSDirectory.Open(path));
            }
        }
    }
}
