using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class AnalyzedField : Field, ISpecializedField
    {
        private const string Suffix = "_anlyzed";

        public bool AnalyzeField => true;
        public string OriginalName { get; }
        public float RelevanceBoostingMultiplier => 1f;

        public Type SpecialAnalyzerType => null;

        public AnalyzedField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) => value;
    }

    internal class AnalyzedFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field) =>
            new AnalyzedField(field.Name, field.Type, field.Relevance, field.EnableFaceting, field.EnableSearching, field.EnableSorting);

        public bool IsEligibleForSpecialization(IField field) =>
                    field.EnableSearching && field.IsString();
    }
}