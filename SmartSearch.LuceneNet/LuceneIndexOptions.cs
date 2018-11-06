using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using System;

namespace SmartSearch.LuceneNet
{
    public class LuceneIndexOptions
    {
        public string DocumentIdFieldName { get; set; } = "__docid";

        public bool ForceCreate { get; set; } = false;

        public string IndexDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        public Func<Analyzer> AnalyzerFactory { get; set; } = () => new StandardAnalyzer(LuceneVersion.LUCENE_48);
    }
}
