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

        public IAnalysisSettings AnalysisSettings { get; private set; }

        public static InternalSearchDomain CreateFrom(ISearchDomain domain)
        {
            var internalDomain = new InternalSearchDomain()
            {
                Name = domain.Name,
                AnalysisSettings = domain.AnalysisSettings ?? new AnalysisSettings(),
                Fields = domain.Fields
            };

            internalDomain.BuildSpecializedFields();

            return internalDomain;
        }

        public IField[] GetFacetEnabledFields() =>
            Fields.Where(f => f.EnableFaceting).ToArray();
        
        void BuildSpecializedFields()
        {
            var specializedFields = new List<ISpecializedField>();
            var specifications = new SpecializedFieldSpecifications(this).ListAll();

            foreach (var spec in specifications)
                foreach (var field in Fields)
                    if (spec.IsEligibleForSpecialization(field))
                        specializedFields.Add(spec.CreateFrom(field));

            SpecializedFields = specializedFields.ToArray();
        }
    }
}
