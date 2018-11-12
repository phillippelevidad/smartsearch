using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;
using SmartSearch.LuceneNet.Internals.Helpers;

namespace SmartSearch.LuceneNet.Internals.Factories
{
    static class IndexWriterFactory
    {
        public static ITaxonomyWriter CreateFacetWriter(InternalSearchDomain domain, LuceneIndexOptions options)
        {
            var facetsPath = IndexDirectoryHelper.GetFacetsDirectoryPath(options.IndexDirectory, domain.Name);

            if (!System.IO.Directory.Exists(facetsPath))
                System.IO.Directory.CreateDirectory(facetsPath);

            return new DirectoryTaxonomyWriter(FSDirectory.Open(facetsPath));
        }

        public static IndexWriter CreateIndexWriter(InternalSearchDomain domain, LuceneIndexOptions options)
        {
            var factory = new InternalAnalyzerFactory(domain, options.AnalyzerFactory);
            var analyzer = factory.Create();

            var config = new IndexWriterConfig(Definitions.LuceneVersion, analyzer)
            {
                OpenMode = options.ForceRecreate ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND
            };

            if (options.IndexInMemory)
            {
                return new IndexWriter(new RAMDirectory(), config);
            }
            else
            {
                var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                return new IndexWriter(FSDirectory.Open(path), config);
            }
        }
    }
}
