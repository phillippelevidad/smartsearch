using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Synonym;
using SmartSearch.LuceneNet.Internals;
using System.IO;

namespace SmartSearch.LuceneNet.Analysis
{
    internal class SynonymsAnalyzer : Analyzer
    {
        private readonly InternalSearchDomain domain;
        private SynonymMap synonymMap;

        public SynonymsAnalyzer(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            if (synonymMap == null)
                BuildSynonymMap();

            var version = Definitions.LuceneVersion;
            var source = new StandardTokenizer(version, reader);

            var stream = new StandardFilter(version, source) as TokenStream;
            stream = new SynonymFilter(stream, synonymMap, true);

            return new TokenStreamComponents(source, stream);
        }

        private void BuildSynonymMap()
        {
            var builder = new SynonymMap.Builder(true);

            if (domain.AnalysisSettings != null && domain.AnalysisSettings.Synonyms != null)
            {
                foreach (var group in domain.AnalysisSettings.Synonyms)
                    foreach (var item in group)
                        foreach (var other in group)
                            if (item != other)
                                builder.Add(item, other);
            }

            synonymMap = builder.Build();
        }
    }
}