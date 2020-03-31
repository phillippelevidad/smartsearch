using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals
{
    internal class InternalSearchDomain : ISearchDomain
    {
        public InternalSearchDomain(string name, IEnumerable<IField> fields, IEnumerable<ISpecializedField> specialiedFields, IAnalysisSettings analysisSettings)
        {
            Name = name;
            Fields = (fields ?? Array.Empty<IField>()).ToList().AsReadOnly();
            SpecializedFields = (specialiedFields ?? Array.Empty<ISpecializedField>()).ToList().AsReadOnly();
            AllFields = Fields.Union(SpecializedFields).ToList().AsReadOnly();
            AnalysisSettings = analysisSettings ?? new AnalysisSettings();
        }

        public string Name { get; }
        public ReadOnlyCollection<IField> Fields { get; }
        public ReadOnlyCollection<ISpecializedField> SpecializedFields { get; }
        public ReadOnlyCollection<IField> AllFields { get; }
        public IAnalysisSettings AnalysisSettings { get; }

        public IField[] GetFacetEnabledFields()
            => Fields.Where(f => f.EnableFaceting).ToArray();

        public static InternalSearchDomain CreateFrom(ISearchDomain domain)
        {
            var specializedFields = BuildSpecializedFields(domain.Fields, domain.AnalysisSettings);
            return new InternalSearchDomain(domain.Name, domain.Fields, specializedFields, domain.AnalysisSettings);
        }

        private static List<ISpecializedField> BuildSpecializedFields(IEnumerable<IField> fields, IAnalysisSettings analysisSettings)
        {
            var specializedFields = new List<ISpecializedField>();
            var specifications = new SpecializedFieldSpecifications(analysisSettings).ListAll();

            foreach (var spec in specifications)
                foreach (var field in fields)
                    if (spec.IsEligibleForSpecialization(field))
                        specializedFields.Add(spec.CreateFrom(field));

            return specializedFields;
        }
    }
}