using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using SmartSearch.LuceneNet.Analysis;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Internals
{
    internal class InternalAnalyzerFactory : IAnalyzerFactory
    {
        private readonly IAnalyzerFactory defaultAnalyzerFactory;
        private readonly InternalSearchDomain domain;

        public InternalAnalyzerFactory(InternalSearchDomain domain, IAnalyzerFactory defaultAnalyzerFactory)
        {
            this.domain = domain;
            this.defaultAnalyzerFactory = defaultAnalyzerFactory;
        }

        public Analyzer Create()
        {
            var fieldAnalyzers = AddSpecializedAnalyzers();
            var defaultAnalyzer = defaultAnalyzerFactory.Create();
            return new PerFieldAnalyzerWrapper(defaultAnalyzer, fieldAnalyzers);
        }

        private Dictionary<string, Analyzer> AddSpecializedAnalyzers()
        {
            var fieldAnalyzers = new Dictionary<string, Analyzer>();
            var knownAnalyzers = new Dictionary<string, Analyzer>
            {
                { typeof(SynonymsAnalyzer).FullName, new SynonymsAnalyzer(domain) }
            };

            foreach (var field in domain.SpecializedFields)
            {
                if (field.SpecialAnalyzerType == null)
                    continue;

                var analyzer = knownAnalyzers[field.SpecialAnalyzerType.FullName];
                fieldAnalyzers.Add(field.Name, analyzer);
            }

            return fieldAnalyzers;
        }
    }
}