using SmartSearch.Abstractions;
using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var domain = CreateDomain();
            var context = CreateContext(domain);
            var provider = CreateDocumentProvider();

            CreateIndex(context, domain, provider);
            Searcher.SearchIndex(context);
        }

        static IIndexContext CreateContext(ISearchDomain domain)
        {
            var baseDir = @"C:\Temp\SmartSearchIndexes\";
            return new PhysicalIndexContext(domain, baseDir, true);
        }

        static IDocumentProvider CreateDocumentProvider()
        {
            return new MegaSoaresDocumentProvider();
        }

        static ISearchDomain CreateDomain()
        {
            return new MegaSoaresSearchDomain();
        }

        static void CreateIndex(IIndexContext context, ISearchDomain domain, IDocumentProvider provider)
        {
            var options = Configuration.GetLuceneIndexOptions();
            var indexService = new LuceneIndexService(options);

            indexService.CreateIndex(context, domain, provider);
        }
    }
}
