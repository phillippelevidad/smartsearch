using SmartSearch;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    partial class Program
    {
        static class Indexer
        {
            public static void CreateIndex()
            {
                var domain = Configuration.SearchDomain;
                var options = Configuration.LuceneIndexOptions;
                var indexService = new LuceneIndexService(options);

                using (var provider = new MockDocumentProvider())
                {
                    indexService.CreateIndex(domain, provider);
                }
            }
        }
    }
}
