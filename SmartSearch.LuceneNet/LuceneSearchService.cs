using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneSearchService : ISearchService
    {
        readonly LuceneIndexOptions options;
        readonly IDocumentConverter documentConverter;

        public LuceneSearchService(LuceneIndexOptions options, IDocumentConverter documentConverter)
        {
            this.options = options;
            this.documentConverter = documentConverter;
        }

        public ISearchResult Search(ISearchDomain domain, ISearchRequest request)
        {
            var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);

            using (var indexDirectory = FSDirectory.Open(path))
            using (var reader = DirectoryReader.Open(indexDirectory))
            {
                var searcher = new IndexSearcher(reader);
                var analyzer = options.AnalyzerFactory.Invoke();

                var defaultField = "Name"; // TODO: find a better way for this. 
                var parser = new QueryParser(Definitions.LuceneVersion, defaultField, analyzer);
                var query = parser.Parse(request.Query);

                return SearchInternal(domain, request, searcher, query);
            }
        }

        ISearchResult SearchInternal(ISearchDomain domain, ISearchRequest request, IndexSearcher searcher, Query query)
        {
            var results = searcher.Search(query, int.MaxValue);
            var hits = results.ScoreDocs;

            int start = request.StartIndex;
            int end = Math.Min(results.TotalHits, request.StartIndex + request.PageSize);

            var items = new IDocument[request.PageSize];
            var itemIndex = 0;

            for (int i = start; i < end; i++)
            {
                var luceneDocument = searcher.Doc(hits[i].Doc);
                var documentResult = documentConverter.Convert(domain, luceneDocument);

                items[itemIndex++] = documentResult;
            }

            return new SearchResult(items, results.TotalHits);
        }
    }
}
