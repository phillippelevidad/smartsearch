using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals
{
    class InternalSearchDomain : ISearchDomain
    {
        public string Name { get; private set; }

        public IField[] Fields { get; private set; }

        public ISpecializedField[] SpecializedFields { get; private set; }

        public IField[] AllFields => Fields.Union(SpecializedFields).ToArray();

        public ISpecializedFieldSpecification[] SpecializedFieldSpecifications { get; private set; }

        public IAnalysisSettings AnalysisSettings { get; private set; }

        public static InternalSearchDomain CreateFrom(ISearchDomain domain)
        {
            var internalDomain = new InternalSearchDomain()
            {
                Name = domain.Name,
                AnalysisSettings = domain.AnalysisSettings ?? new AnalysisSettings(),
                Fields = domain.Fields
            };

            internalDomain.BuildSpecializedFieldSpecifications();
            internalDomain.BuildSpecializedFields();

            return internalDomain;
        }

        public IField[] GetFacetEnabledFields() =>
            Fields.Where(f => f.EnableFaceting).ToArray();

        void BuildSpecializedFieldSpecifications()
        {
            var specifications = new List<ISpecializedFieldSpecification>
            {
                new AnalyzedFieldSpecification()
            };

            if (AnalysisSettings?.Synonyms?.Any() ?? false)
                specifications.Add(new SynonymFieldSpecification());

            SpecializedFieldSpecifications = specifications.ToArray();
        }

        void BuildSpecializedFields()
        {
            var specializedFields = new List<ISpecializedField>();

            foreach (var spec in SpecializedFieldSpecifications)
                foreach (var field in Fields)
                    if (spec.IsEligibleToBecomeSpecialized(field))
                        specializedFields.Add(spec.BuildFrom(field));

            SpecializedFields = specializedFields.ToArray();
        }
    }
}
