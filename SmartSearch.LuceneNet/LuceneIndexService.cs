using Lucene.Net.Facet;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Builders;
using SmartSearch.LuceneNet.Internals.Helpers;
using SmartSearch.LuceneNet.Internals.IndexFactories;
using System;
using System.IO;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexService : IIndexService
    {
        private readonly LuceneIndexOptions options;
        private FacetsConfig facetsConfig;

        public LuceneIndexService() : this(new LuceneIndexOptions()) { }

        public LuceneIndexService(LuceneIndexOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void CreateIndex(
            IIndexContext context,
            ISearchDomain domain,
            IDocumentProvider documentProvider
        )
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            if (documentProvider == null)
                throw new ArgumentNullException(nameof(documentProvider));

            new SearchDomainValidator().Validate(domain);

            try
            {
                using (CultureContext.Invariant)
                {
                    var contextWrapper = new IndexContextWrapper(context);
                    var internalDomain = InternalSearchDomain.CreateFrom(domain);

                    SetFacetsConfig(internalDomain);

                    using (
                        var facetWriter = IndexWriterFactory.CreateFacetWriter(
                            contextWrapper,
                            internalDomain,
                            options
                        )
                    )
                    using (
                        var indexWriter = IndexWriterFactory.CreateIndexWriter(
                            contextWrapper,
                            internalDomain,
                            options
                        )
                    )
                    using (var documentReader = documentProvider.GetDocumentReader())
                    {
                        var documentBuilder = new IndexDocumentBuilder(
                            internalDomain,
                            facetsConfig,
                            facetWriter
                        );

                        while (documentReader.ReadNext())
                            HandleDocument(
                                indexWriter,
                                documentBuilder,
                                documentReader.CurrentDocument
                            );
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ErrorCreatingLuceneIndexException(ex);
            }
        }

        private Term GetUpdateOrDeleteDocumentClause(string documentId) =>
            new Term(Definitions.DocumentIdFieldName, documentId);

        private void HandleDocument(
            IndexWriter writer,
            IndexDocumentBuilder builder,
            IDocumentOperation document
        )
        {
            switch (document.OperationType)
            {
                case DocumentOperationType.AddOrUpdate:
                    HandleDocumentAddOrUpdate(writer, builder, document);
                    break;

                case DocumentOperationType.Delete:
                    HandleDocumentDelete(writer, builder, document);
                    break;

                default:
                    throw new UnknownDocumentOperationTypeException(document.OperationType);
            }
        }

        private void HandleDocumentAddOrUpdate(
            IndexWriter writer,
            IndexDocumentBuilder builder,
            IDocumentOperation document
        )
        {
            var indexDocument = builder.Build(document);

            if (writer.Config.OpenMode == OpenMode.CREATE)
                writer.AddDocument(indexDocument);
            else
            {
                var updateClause = GetUpdateOrDeleteDocumentClause(document.Id);
                writer.UpdateDocument(updateClause, indexDocument);
            }
        }

        private void HandleDocumentDelete(
            IndexWriter writer,
            IndexDocumentBuilder builder,
            IDocumentOperation document
        )
        {
            if (writer.Config.OpenMode != OpenMode.CREATE)
            {
                var deleteClause = GetUpdateOrDeleteDocumentClause(document.Id);
                writer.DeleteDocuments(deleteClause);
            }
        }

        private void SetFacetsConfig(InternalSearchDomain domain)
        {
            facetsConfig = new FacetsConfig();

            foreach (var field in domain.Fields)
                if (field.EnableFaceting)
                    facetsConfig.SetMultiValued(field.Name, true);
        }
    }
}
