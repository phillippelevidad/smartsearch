using SmartSearch.LuceneNet;
using SmartSearch.LuceneNet.Analysis;

namespace ConsoleApp
{
    static class Configuration
    {
        public static LuceneIndexOptions GetLuceneIndexOptions() => new LuceneIndexOptions()
            .UseAnalyzerFactory(new BrazilianAnalyzerFactory());
    }
}
