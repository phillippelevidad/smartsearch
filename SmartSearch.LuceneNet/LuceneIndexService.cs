using Lucene.Net.Index;
using Lucene.Net.Store;
using SmartSearch.Abstractions;
using System;
using System.IO;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexService : IIndexService
    {
        readonly LuceneIndexOptions options;
        readonly IDocumentConverter documentConverter;

        public LuceneIndexService() : this(new LuceneIndexOptions(), new DefaultDocumentConverter())
        {
        }

        public LuceneIndexService(LuceneIndexOptions options) : this(options, new DefaultDocumentConverter())
        {
        }

        public LuceneIndexService(LuceneIndexOptions options, IDocumentConverter documentConverter)
        {
            this.options = options ??
                throw new ArgumentNullException(nameof(options));

            this.documentConverter = documentConverter ??
                throw new ArgumentNullException(nameof(documentConverter));
        }

        public void CreateIndex(ISearchDomain domain, IDocumentProvider documentProvider)
        {
            try
            {
                var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var analyzer = options.AnalyzerFactory.Create();
                var config = new IndexWriterConfig(Definitions.LuceneVersion, analyzer)
                {
                    OpenMode = options.ForceCreate ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND
                };

                using (var indexDirectory = FSDirectory.Open(path))
                using (var indexWriter = new IndexWriter(indexDirectory, config))
                using (var documentReader = documentProvider.GetDocumentReader())
                {
                    while (documentReader.ReadNext())
                    {
                        var document = documentReader.CurrentDocument;
                        var luceneDocument = documentConverter.Convert(domain, document);

                        if (indexWriter.Config.OpenMode == OpenMode.CREATE)
                            indexWriter.AddDocument(luceneDocument);

                        else
                            indexWriter.UpdateDocument(
                                new Term(Definitions.DocumentIdFieldName, document.Id), luceneDocument);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ErrorCreatingLuceneIndexException(ex);
            }
        }
    }
}
