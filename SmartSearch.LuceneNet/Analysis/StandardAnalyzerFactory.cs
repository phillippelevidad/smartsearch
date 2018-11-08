using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using SmartSearch.LuceneNet.Internals;

namespace SmartSearch.LuceneNet.Analysis
{
    public class StandardAnalyzerFactory : IAnalyzerFactory
    {
        public Analyzer Create() => new StandardAnalyzer(Definitions.LuceneVersion);
    }
}
