using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexOptions
    {
        public bool ForceCreate { get; set; } = false;

        public string IndexDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public IAnalyzerFactory AnalyzerFactory { get; set; } = new StandardAnalyzerFactory();
    }
}
