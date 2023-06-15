using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;
using System;
using System.Globalization;
using System.Text;

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
            : base(name + Suffix, type, relevance, false, true, true, false)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) =>
            value == null ? null : Normalize(value.ToString());

        private string Normalize(string text)
        {
            text = text.ToLowerInvariant();

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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
