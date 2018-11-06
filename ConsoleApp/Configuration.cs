using SmartSearch;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet;

namespace ConsoleApp
{
    static class Configuration
    {
        public static ISearchDomain SearchDomain => new SearchDomain("testindex", new[]
        {
            new Field("Id", FieldType.Literal, enableSearching: true, enableReturning: true),
            new Field("Name", FieldType.Text, enableSearching: true, enableReturning: true),
            new Field("Color", FieldType.Literal, enableFaceting: true, enableSearching: true, enableReturning: true),
            new Field("Sizes", FieldType.LiteralArray, enableFaceting: true, enableSearching: true, enableReturning: true)
        });

        public static LuceneIndexOptions LuceneIndexOptions => new LuceneIndexOptions
        {
            ForceCreate = true,
            IndexDirectory = @"C:\Temp\SmartSearchIndexes\"
        };
    }
}
