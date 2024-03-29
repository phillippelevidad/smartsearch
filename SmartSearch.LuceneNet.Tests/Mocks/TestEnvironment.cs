﻿using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    internal class TestEnvironment
    {
        public IDocumentProvider DocumentProvider { get; set; }
        public IDocument[] Documents { get; set; }

        public IIndexContext IndexContext { get; set; }
        public IIndexService IndexService { get; set; }
        public LuceneIndexOptions Options { get; set; }
        public ISearchDomain SearchDomain { get; set; }
        public ISearchService SearchService { get; set; }

        public static TestEnvironment Build(bool createIndex = true)
        {
            var documents = MockDocuments.ListAll();
            var options = new LuceneIndexOptions().UseAnalyzerFactory(new BrazilianAnalyzerFactory());

            var env = new TestEnvironment
            {
                Documents = documents,
                DocumentProvider = new DocumentProvider(documents),

                SearchDomain = new MockSearchDomain(),
                IndexContext = new MemoryIndexContext(), // new PhysicalIndexContext(@"C:\Temp\SmartSearchIndexes\testlatlng", true)
                IndexService = new LuceneIndexService(options),
                SearchService = new LuceneSearchService(options),

                Options = options
            };

            if (createIndex)
                env.CreateIndex();

            return env;
        }

        public void CreateIndex()
        {
            IndexService.CreateIndex(IndexContext, SearchDomain, DocumentProvider);
        }

        public ISearchResult Search(ISearchRequest request)
        {
            return SearchService.Search(IndexContext, SearchDomain, request);
        }
    }
}