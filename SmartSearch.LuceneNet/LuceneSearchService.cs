using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Converters;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneSearchService : ISearchService
    {
        readonly LuceneIndexOptions options;
        readonly IDocumentConverter documentConverter;

        public LuceneSearchService() : this(new LuceneIndexOptions())
        {
        }

        public LuceneSearchService(LuceneIndexOptions options)
        {
            this.options = options;
            documentConverter = new DefaultDocumentConverter();
        }

        public ISearchResult Search(ISearchDomain domain, ISearchRequest request)
        {
            var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);

            using (var indexDirectory = FSDirectory.Open(path))
            using (var reader = DirectoryReader.Open(indexDirectory))
            {
                var searcher = new IndexSearcher(reader);
                var query = QueryGenerator.GetQuery(domain, request, options.AnalyzerFactory);
                var sort = SortGenerator.GetSort(domain, request);

                return SearchInternal(domain, request, searcher, query, sort);
            }
        }

        ISearchResult SearchInternal(ISearchDomain domain, ISearchRequest request, IndexSearcher searcher, Query query, Sort sort)
        {
            var results = searcher.Search(query, int.MaxValue, sort);
            var hits = results.ScoreDocs;

            var start = request.StartIndex;
            var end = Math.Min(results.TotalHits, request.StartIndex + request.PageSize);
            var resultSize = end - start;

            var items = new IDocument[resultSize];
            var itemIndex = 0;

            for (var i = start; i < end; i++)
            {
                var luceneDocument = searcher.Doc(hits[i].Doc);
                var documentResult = documentConverter.Convert(domain, luceneDocument);

                items[itemIndex++] = documentResult;
            }

            return new SearchResult(items, results.TotalHits);
        }
    }
}
