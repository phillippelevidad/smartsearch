using SmartSearch.LuceneNet.Analysis;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexOptions
    {
        public IAnalyzerFactory AnalyzerFactory { get; private set; } = new StandardAnalyzerFactory();

        #region Fluent API

        public LuceneIndexOptions UseAnalyzerFactory(IAnalyzerFactory analyzerFactory)
        {
            AnalyzerFactory = analyzerFactory ?? throw new ArgumentNullException(nameof(analyzerFactory));
            return this;
        }

        #endregion
    }
}
