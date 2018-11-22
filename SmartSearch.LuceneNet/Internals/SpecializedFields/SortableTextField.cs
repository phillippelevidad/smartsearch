using SmartSearch.Abstractions;
using System;
using System.Globalization;
using System.Text;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class SortableTextField : Field, ISpecializedField
    {
        const string Suffix = "_srt";

        public string OriginalName { get; }

        public Type AnalyzerType => null;

        public bool AnalyzeField => false;

        public SortableTextField(string name, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name + Suffix, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) => value == null
            ? null : Normalize(value.ToString());

        string Normalize(string text)
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

    class SortableTextFieldSpecification : ISpecializedFieldSpecification
    {
        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSorting && (field.Type == FieldType.Text || field.Type == FieldType.TextArray);

        public ISpecializedField CreateFrom(IField field)
        {
            var newFieldType =
                field.Type == FieldType.Text ? FieldType.Literal :
                field.Type == FieldType.TextArray ? FieldType.LiteralArray :
                throw new ArgumentException();

            return new SortableTextField(field.Name, newFieldType, field.Relevance,
                field.EnableFaceting, field.EnableSearching, field.EnableSorting);
        }
    }
}
