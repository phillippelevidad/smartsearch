using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using SmartSearch.LuceneNet.Analysis;
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
            var fieldAnalyzers = AddSpecializedAnalyzers();
            var defaultAnalyzer = defaultAnalyzerFactory.Create();
            return new PerFieldAnalyzerWrapper(defaultAnalyzer, fieldAnalyzers);
        }

        Dictionary<string, Analyzer>  AddSpecializedAnalyzers()
        {
            var fieldAnalyzers = new Dictionary<string, Analyzer>();
            var knownAnalyzers = new Dictionary<string, Analyzer>
            {
                { typeof(SynonymsAnalyzer).FullName, new SynonymsAnalyzer(domain) }
            };

            foreach (var spec in domain.SpecializedFieldSpecifications)
            {
                foreach (var field in domain.SpecializedFields)
                {
                    if (spec.IsSatisfiedBy(field))
                    {
                        var type = spec.GetAnalyzerType();

                        if (type == null)
                            continue;

                        var analyzer = knownAnalyzers[type.FullName];
                        fieldAnalyzers.Add(field.Name, analyzer);
                    }
                }
            }

            return fieldAnalyzers;
        }
    }
}
