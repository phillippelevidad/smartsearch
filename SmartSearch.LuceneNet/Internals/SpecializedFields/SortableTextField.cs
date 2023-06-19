using SmartSearch.Abstractions;
using System;
using System.Globalization;
using System.Text;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class SortableTextField : Field, ISpecializedField
    {
        private const string Suffix = "_srt";

        public bool AnalyzeField => false;
        public string OriginalName { get; }
        public float RelevanceBoostingMultiplier => 1f;

        public Type SpecialAnalyzerType => null;

        public SortableTextField(string name, FieldType type, FieldRelevance relevance)
            : base(
                name + Suffix,
                type,
                relevance,
                enableFaceting: false,
                enableSearching: false,
                enableSorting: true
            )
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value)
        {
            if (value == null)
                return null;
            return Normalize(value.ToString());
        }

        private string Normalize(string input)
        {
            var normalizedText = RemoveDiacritics(input);
            normalizedText = RemoveNonAlphanumeric(normalizedText);
            normalizedText = normalizedText.ToLowerInvariant();
            return normalizedText;
        }

        private string RemoveDiacritics(string input)
        {
            var normalizedText = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        private string RemoveNonAlphanumeric(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in input)
            {
                if (IsAlphanumeric(c))
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        private bool IsAlphanumeric(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }
    }

    internal class SortableTextFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field)
        {
            var newFieldType =
                field.Type == FieldType.Text
                    ? FieldType.Literal
                    : field.Type == FieldType.TextArray
                        ? FieldType.LiteralArray
                        : throw new ArgumentException();

            return new SortableTextField(field.Name, newFieldType, field.Relevance);
        }

        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSorting
            && (field.Type == FieldType.Text || field.Type == FieldType.TextArray);
    }
}
