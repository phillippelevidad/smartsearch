using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Br;
using Lucene.Net.Analysis.Standard;
using SmartSearch.LuceneNet.Internals;

namespace SmartSearch.LuceneNet
{
    public interface IAnalyzerFactory
    {
        Analyzer Create();
    }

    public class StandardAnalyzerFactory : IAnalyzerFactory
    {
        public Analyzer Create() => new StandardAnalyzer(Definitions.LuceneVersion);
    }

    public class BrazilianAnalyzerFactory : IAnalyzerFactory
    {
        public Analyzer Create() => new BrazilianAnalyzer(Definitions.LuceneVersion);
    }
}
