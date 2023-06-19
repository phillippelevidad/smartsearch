using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class PhoneticField : Field, ISpecializedField
    {
        private const string Suffix = "_phon";

        public bool AnalyzeField => true;
        public string OriginalName { get; }
        public new float RelevanceModifier => FieldRelevanceBoost.GetBoostValue(Relevance) * 0.1f;

        public Type SpecialAnalyzerType => typeof(PhoneticAnalyzer);

        public PhoneticField(string name, FieldType type, FieldRelevance relevance)
            : base(
                name + Suffix,
                type,
                relevance,
                enableFaceting: false,
                enableSearching: true,
                enablePhoneticSearch: true,
                enableSorting: false
            )
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value)
        {
            return value?.ToString();
        }
    }

    internal class PhoneticFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field)
        {
            var newFieldType =
                field.Type == FieldType.Text
                    ? FieldType.Literal
                    : field.Type == FieldType.TextArray
                        ? FieldType.LiteralArray
                        : throw new ArgumentException();

            return new PhoneticField(field.Name, newFieldType, field.Relevance);
        }

        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSearching
            && field.EnablePhoneticSearch
            && (field.Type == FieldType.Text || field.Type == FieldType.TextArray);
    }
}
