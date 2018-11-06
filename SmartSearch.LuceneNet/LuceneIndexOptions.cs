using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexOptions
    {
        public bool ForceCreate { get; set; } = false;

        public string IndexDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public Func<Analyzer> AnalyzerFactory { get; set; } = () => new StandardAnalyzer(Definitions.LuceneVersion);
    }
}
