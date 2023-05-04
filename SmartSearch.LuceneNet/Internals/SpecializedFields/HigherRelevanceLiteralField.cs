using Lucene.Net.Analysis.Core;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Analysis;
using System;
using System.Globalization;
using System.Text;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class HigherRelevanceLiteralField : Field, ISpecializedField
    {
        private const string Suffix = "_lit";

        public bool AnalyzeField => true;
        public string OriginalName { get; }
        public new float RelevanceBoost => FieldRelevanceBoost.GetBoostValue(Relevance) * 3f;

        public Type SpecialAnalyzerType => typeof(AlmostExactMatchAnalyzer);

        public HigherRelevanceLiteralField(string name, FieldType type, FieldRelevance relevance)
            : base(name + Suffix, type, relevance, false, true, false)
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

    /// <summary>
    /// Creates a literal field with a high relevance boost.
    /// </summary>
    internal class HigherRelevanceLiteralFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field)
        {
            var newFieldType =
                field.Type == FieldType.Text ? FieldType.Literal :
                field.Type == FieldType.TextArray ? FieldType.LiteralArray :
                throw new ArgumentException();
            
            return new HigherRelevanceLiteralField(field.Name, newFieldType, field.Relevance);
        }

        public bool IsEligibleForSpecialization(IField field) =>
            field.EnableSearching &&
            field.Relevance == FieldRelevance.Higher &&
            (field.Type == FieldType.Text || field.Type == FieldType.TextArray);
    }
}