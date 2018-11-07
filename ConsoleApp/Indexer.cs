using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    partial class Program
    {
        static class Indexer
        {
            public static void CreateIndex()
            {
                var domain = new PromonetSearchDomain();
                var options = Configuration.GetLuceneIndexOptions();
                var indexService = new LuceneIndexService(options);

                using (var provider = new PromonetDocumentProvider())
                    indexService.CreateIndex(domain, provider);
            }
        }
    }
}
