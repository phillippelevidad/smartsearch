using SmartSearch.Abstractions;
using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    partial class Program
    {
        private static IIndexContext CreateContext(ISearchDomain domain)
        {
            var indexDirectory = System.IO.Path.Combine(@"C:\Temp\SmartSearchIndexes", domain.Name);
            return new PhysicalIndexContext(indexDirectory, true);
        }

        private static IDocumentProvider CreateDocumentProvider()
        {
            return new MegaSoaresDocumentProvider();
        }

        private static ISearchDomain CreateDomain()
        {
            return new MegaSoaresSearchDomain();
        }

        private static void CreateIndex(IIndexContext context, ISearchDomain domain, IDocumentProvider provider)
        {
            var options = Configuration.GetLuceneIndexOptions();
            var indexService = new LuceneIndexService(options);

            indexService.CreateIndex(context, domain, provider);
        }

        private static void Main(string[] args)
        {
            var domain = CreateDomain();
            var context = CreateContext(domain);
            var provider = CreateDocumentProvider();

            CreateIndex(context, domain, provider);
            Searcher.SearchIndex(context);
        }
    }
}