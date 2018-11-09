using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    partial class Program
    {
        static class Indexer
        {
            public static void CreateIndex()
            {
                var domain = new MegaSoaresSearchDomain();
                var options = Configuration.GetLuceneIndexOptions();
                var indexService = new LuceneIndexService(options);

                using (var provider = new MegaSoaresDocumentProvider())
                    indexService.CreateIndex(domain, provider);
            }
        }
    }
}
