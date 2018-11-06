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
                var domain = new SearchDomain("testindex", new[]
                {
                    new Field("Id", FieldType.Literal, enableSearching: true, enableReturning: true),
                    new Field("Name", FieldType.Text, enableSearching: true, enableReturning: true),
                    new Field("Color", FieldType.Literal, enableFaceting: true, enableSearching: true, enableReturning: true),
                    new Field("Sizes", FieldType.LiteralArray, enableFaceting: true, enableSearching: true, enableReturning: true)
                });

                var indexService = new LuceneIndexService(new LuceneIndexOptions
                {
                    IndexDirectory = @"C:\Temp\SmartSearchIndexes\"
                });

                using (var provider = new MockDocumentProvider())
                {
                    indexService.CreateIndex(domain, provider);
                }
            }
        }
    }
}
