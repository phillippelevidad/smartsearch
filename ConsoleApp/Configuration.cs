using SmartSearch.LuceneNet;
using SmartSearch.LuceneNet.Analysis;

namespace ConsoleApp
{
    static class Configuration
    {
        public static LuceneIndexOptions GetLuceneIndexOptions() => new LuceneIndexOptions
        {
            ForceCreate = true,
            IndexDirectory = @"C:\Temp\SmartSearchIndexes\",
            AnalyzerFactory = new BrazilianAnalyzerFactory()
        };
    }
}
