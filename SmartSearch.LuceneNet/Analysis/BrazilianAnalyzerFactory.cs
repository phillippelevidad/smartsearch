using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Br;
using SmartSearch.LuceneNet.Internals;

namespace SmartSearch.LuceneNet.Analysis
{
    public class BrazilianAnalyzerFactory : IAnalyzerFactory
    {
        public Analyzer Create() => new BrazilianAnalyzer(Definitions.LuceneVersion);
    }
}
