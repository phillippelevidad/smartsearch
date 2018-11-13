using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Converters;
using SmartSearch.LuceneNet.Internals.Factories;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;
using System.IO;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexService : IIndexService
    {
        FacetsConfig facetsConfig;

        readonly LuceneIndexOptions options;
        readonly IDocumentConverter defaultDocumentConverter;
        readonly IDocumentConverter facetDocumentConverter;

        public LuceneIndexService() : this(new LuceneIndexOptions())
        {
        }

        public LuceneIndexService(LuceneIndexOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            defaultDocumentConverter = new DefaultDocumentConverter();
            facetDocumentConverter = new FacetDocumentConverter();
        }

        public void CreateIndex(IIndexContext context, ISearchDomain domain, IDocumentProvider documentProvider)
        {
            try
            {
                using (CultureContext.Invariant)
                {
                    var contextWrapper = new IndexContextWrapper(context);
                    var internalDomain = InternalSearchDomain.CreateFrom(domain);

                    SetFacetsConfig(internalDomain);

                    using (var facetWriter = IndexWriterFactory.CreateFacetWriter(contextWrapper, internalDomain, options))
                    using (var indexWriter = IndexWriterFactory.CreateIndexWriter(contextWrapper, internalDomain, options))
                    using (var documentReader = documentProvider.GetDocumentReader())
                    {
                        while (documentReader.ReadNext())
                        {
                            var document = BuildDocument(internalDomain, documentReader.CurrentDocument, facetWriter);

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
            }
            catch (IOException ex)
            {
                throw new ErrorCreatingLuceneIndexException(ex);
            }
        }

        LuceneDocument BuildDocument(InternalSearchDomain domain, IDocument document, ITaxonomyWriter taxonomyWriter)
        {
            var internalDocument = InternalDocument.CreateFrom(domain, document);
            var mainDocument = defaultDocumentConverter.Convert(domain, internalDocument);
            var facetDocument = facetDocumentConverter.Convert(domain, internalDocument);

            foreach (var facet in facetDocument.Fields)
                mainDocument.Add(facet);

            return facetsConfig.Build(taxonomyWriter, mainDocument);
        }

        void SetFacetsConfig(InternalSearchDomain domain)
        {
            facetsConfig = new FacetsConfig();

            foreach (var field in domain.Fields)
                if (field.EnableFaceting)
                    facetsConfig.SetMultiValued(field.Name, true);
        }
    }
}
