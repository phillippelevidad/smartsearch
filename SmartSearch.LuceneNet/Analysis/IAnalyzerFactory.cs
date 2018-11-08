using Lucene.Net.Analysis;

namespace SmartSearch.LuceneNet.Analysis
{
    public interface IAnalyzerFactory
    {
        Analyzer Create();
    }
}
