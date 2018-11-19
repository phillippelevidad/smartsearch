using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using SmartSearch.LuceneNet.Analysis;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Internals
{
    class InternalAnalyzerFactory : IAnalyzerFactory
    {
        readonly InternalSearchDomain domain;
        readonly IAnalyzerFactory defaultAnalyzerFactory;

        public InternalAnalyzerFactory(InternalSearchDomain domain, IAnalyzerFactory defaultAnalyzerFactory)
        {
            this.domain = domain;
            this.defaultAnalyzerFactory = defaultAnalyzerFactory;
        }

        public Analyzer Create()
        {
            var defaultAnalyzer = defaultAnalyzerFactory.Create();
            var fieldAnalyzers = new Dictionary<string, Analyzer>();

            AddSynonymsAnalyzer(fieldAnalyzers);

            return new PerFieldAnalyzerWrapper(defaultAnalyzer, fieldAnalyzers);
        }

        void AddSynonymsAnalyzer(Dictionary<string, Analyzer> fieldAnalyzers)
        {
            var synonymSpec = new SynonymFieldSpecification(); // Bring the knowledge of which analyzer to use into the specification itself, making it the absolute referente for this stuff
            var synonymAnalyzer = new SynonymsAnalyzer(domain);

            foreach (var field in domain.SpecializedFields)
                if (synonymSpec.IsSatisfiedBy(field))
                    fieldAnalyzers.Add(field.Name, synonymAnalyzer);
        }
    }
}
