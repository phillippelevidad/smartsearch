using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class SynonymField : Field, ISpecializedField
    {
        const string Suffix = "_syn";

        public string OriginalName { get; }

        public Type SpecialAnalyzerType => typeof(SynonymsAnalyzer);

        public bool AnalyzeField => true;

        public SynonymField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) => value;

    }

    class SynonymFieldSpecification : ISpecializedFieldSpecification
    {
        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSearching && field.IsString();

        public ISpecializedField CreateFrom(IField field) =>
            new SynonymField(field.Name, field.Type, field.Relevance, field.EnableFaceting, field.EnableSearching, field.EnableSorting);
    }
}
