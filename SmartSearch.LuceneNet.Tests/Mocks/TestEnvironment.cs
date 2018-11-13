using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    class TestEnvironment
    {
        public IDocument[] Documents { get; set; }

        public ISearchDomain SearchDomain { get; set; }

        public IDocumentProvider DocumentProvider { get; set; }

        public IIndexContext IndexContext { get; set; }

        public IIndexService IndexService { get; set; }

        public ISearchService SearchService { get; set; }

        public LuceneIndexOptions Options { get; set; }

        public ISearchResult Search(ISearchRequest request)
        {
            return SearchService.Search(IndexContext, SearchDomain, request);
        }

        public static TestEnvironment Build()
        {
            var documents = MockDocuments.ListAll();
            var options = new LuceneIndexOptions().UseAnalyzerFactory(new BrazilianAnalyzerFactory());

            var env = new TestEnvironment
            {
                Documents = documents,
                DocumentProvider = new MockDocumentProvider(documents),

                SearchDomain = new MockSearchDomain(),
                IndexContext = new InMemoryIndexContext(),
                IndexService = new LuceneIndexService(options),
                SearchService = new LuceneSearchService(options),

                Options = options
            };

            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, env.DocumentProvider);

            return env;
        }
    }
}
