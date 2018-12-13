using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class SynonymField : Field, ISpecializedField
    {
        private const string Suffix = "_syn";

        public bool AnalyzeField => true;
        public string OriginalName { get; }

        public Type SpecialAnalyzerType => typeof(SynonymsAnalyzer);

        public SynonymField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) => value;
    }

    internal class SynonymFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field) =>
            new SynonymField(field.Name, field.Type, field.Relevance, field.EnableFaceting, field.EnableSearching, field.EnableSorting);

        public bool IsEligibleForSpecialization(IField field) =>
                    field.EnableSearching && field.IsString();
    }
}