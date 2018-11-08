using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Store;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Converters;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;
using System.IO;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexService : IIndexService
    {
        readonly FacetsConfig facetsConfig;
        readonly LuceneIndexOptions options;

        readonly IDocumentConverter defaultDocumentConverter;
        readonly IDocumentConverter facetDocumentConverter;

        public LuceneIndexService() : this(new LuceneIndexOptions())
        {
        }

        public LuceneIndexService(LuceneIndexOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            facetsConfig = new FacetsConfig();
            defaultDocumentConverter = new DefaultDocumentConverter();
            facetDocumentConverter = new FacetDocumentConverter();
        }

        public void CreateIndex(ISearchDomain domain, IDocumentProvider documentProvider)
        {
            try
            {
                using (var facetWriter = GetFacetWriter(domain))
                using (var indexWriter = GetIndexWriter(domain))
                using (var documentReader = documentProvider.GetDocumentReader())
                {
                    while (documentReader.ReadNext())
                    {
                        var document = BuildDocument(domain, documentReader.CurrentDocument, facetWriter);

                        if (indexWriter.Config.OpenMode == OpenMode.CREATE)
                            indexWriter.AddDocument(document);

                        else
                        {
                            var updateClause = new Term(Definitions.DocumentIdFieldName, documentReader.CurrentDocument.Id);
                            indexWriter.UpdateDocument(updateClause, document);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ErrorCreatingLuceneIndexException(ex);
            }
        }

        LuceneDocument BuildDocument(ISearchDomain domain, IDocument document, ITaxonomyWriter taxonomyWriter)
        {
            var mainDocument = defaultDocumentConverter.Convert(domain, document);
            var facetDocument = facetDocumentConverter.Convert(domain, document);

            foreach (var facet in facetDocument.Fields)
                mainDocument.Add(facet);

            return facetsConfig.Build(taxonomyWriter, mainDocument);
        }

        ITaxonomyWriter GetFacetWriter(ISearchDomain domain)
        {
            var facetsPath = IndexDirectoryHelper.GetFacetsDirectoryPath(options.IndexDirectory, domain.Name);

            if (!System.IO.Directory.Exists(facetsPath))
                System.IO.Directory.CreateDirectory(facetsPath);

            return new DirectoryTaxonomyWriter(FSDirectory.Open(facetsPath));
        }

        IndexWriter GetIndexWriter(ISearchDomain domain)
        {
            var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            var config = new IndexWriterConfig(Definitions.LuceneVersion, options.AnalyzerFactory.Create())
            {
                OpenMode = options.ForceCreate ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND
            };

            return new IndexWriter(FSDirectory.Open(path), config);
        }
    }
}
