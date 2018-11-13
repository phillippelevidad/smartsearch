using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;

namespace SmartSearch.LuceneNet.Internals.Factories
{
    static class IndexWriterFactory
    {
        public static ITaxonomyWriter CreateFacetWriter(IndexContextWrapper contextWrapper, InternalSearchDomain domain, LuceneIndexOptions options)
        {
            return new DirectoryTaxonomyWriter(contextWrapper.FacetsDirectory);
        }

        public static IndexWriter CreateIndexWriter(IndexContextWrapper contextWrapper, InternalSearchDomain domain, LuceneIndexOptions options)
        {
            var factory = new InternalAnalyzerFactory(domain, options.AnalyzerFactory);
            var analyzer = factory.Create();

            var mode = GetOpenMode(contextWrapper);
            var config = new IndexWriterConfig(Definitions.LuceneVersion, analyzer) { OpenMode = mode };

            return new IndexWriter(contextWrapper.IndexDirectory, config);
        }

        static OpenMode GetOpenMode(IndexContextWrapper contextWrapper)
        {
            var forceRecreate = false;

            if (contextWrapper.WrappedContext is PhysicalIndexContext physicalContext)
                forceRecreate = physicalContext.ForceRecreate;

            return forceRecreate ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND;
        }
    }
}
