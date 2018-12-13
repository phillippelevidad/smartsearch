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

        public Type SpecialAnalyzerType => null;

        public SortableTextField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) => value == null
            ? null : Normalize(value.ToString());

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

    internal class SortableTextFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field)
        {
            var newFieldType =
                field.Type == FieldType.Text ? FieldType.Literal :
                field.Type == FieldType.TextArray ? FieldType.LiteralArray :
                throw new ArgumentException();

            return new SortableTextField(field.Name, newFieldType, field.Relevance,
                field.EnableFaceting, field.EnableSearching, field.EnableSorting);
        }

        public bool IsEligibleForSpecialization(IField field) =>
                    field.EnableSorting && (field.Type == FieldType.Text || field.Type == FieldType.TextArray);
    }
}