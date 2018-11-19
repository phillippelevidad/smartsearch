using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class AnalyzedField : Field, ISpecializedField
    {
        const string Suffix = "_anlyzed";

        public string OriginalName { get; }

        public Type AnalyzerType => null;

        public AnalyzedField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }
    }

    class AnalyzedFieldSpecification : ISpecializedFieldSpecification
    {
        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSearching && field.IsString();

        public ISpecializedField CreateFrom(IField field) =>
            new AnalyzedField(field.Name, field.Type, field.Relevance, field.EnableFaceting, field.EnableSearching, field.EnableSorting);
    }
}
