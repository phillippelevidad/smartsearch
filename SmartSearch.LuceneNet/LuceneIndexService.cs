using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
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

        public void CreateIndex(IDomain domain, IDocumentProvider documentProvider)
        {
            try
            {
                var path = GetIndexDirectoryPath(domain.Name);

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                var indexDirectory = FSDirectory.Open(path);
                var analyzer = options.AnalyzerFactory.Invoke();
                var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer)
                {
                    OpenMode = options.ForceCreate ? OpenMode.CREATE : OpenMode.CREATE_OR_APPEND
                };

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
                                new Term(options.DocumentIdFieldName, document.Id), luceneDocument);
                    }
                }
            }
            catch (IOException ex)
            {
                throw new ErrorCreatingLuceneIndexException(ex);
            }
        }

        string GetIndexDirectoryPath(string domainName) => Path.Combine(options.IndexDirectory, domainName);
    }
}
