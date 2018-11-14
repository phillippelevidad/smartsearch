using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Search;
using Lucene.Net.Util;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class BoolFilterBuilder : TypedFilterBuilderBase
    {
        public BoolFilterBuilder() : base(FieldType.Bool, FieldType.BoolArray) { }

        protected override Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = BoolConverter.ConvertToInt(filter.RangeFrom ?? false);
            var to = BoolConverter.ConvertToInt(filter.RangeTo ?? false);
            return NumericRangeFilter.NewInt32Range(field.Name, from, to, true, true);
        }

        protected override Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = BoolConverter.ConvertToInt(filter.SingleValue);
            return NumericRangeFilter.NewInt32Range(field.Name, value, value, true, true);
        }
    }

    class DateFilterBuilder : TypedFilterBuilderBase
    {
        public DateFilterBuilder() : base(FieldType.Date, FieldType.DateArray) { }

        protected override Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DateTimeConverter.ConvertToLong(filter.RangeFrom ?? DateTime.MinValue);
            var to = DateTimeConverter.ConvertToLong(filter.RangeTo ?? DateTime.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DateTimeConverter.ConvertToLong(filter.SingleValue);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    class DoubleFilterBuilder : TypedFilterBuilderBase
    {
        public DoubleFilterBuilder() : base(FieldType.Double, FieldType.DoubleArray) { }

        protected override Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DoubleConverter.Convert(filter.RangeFrom ?? double.MinValue);
            var to = DoubleConverter.Convert(filter.RangeTo ?? double.MaxValue);
            return NumericRangeFilter.NewDoubleRange(field.Name, from, to, true, true);
        }

        protected override Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DoubleConverter.Convert(filter.SingleValue);
            return NumericRangeFilter.NewDoubleRange(field.Name, value, value, true, true);
        }
    }

    class IntFilterBuilder : TypedFilterBuilderBase
    {
        public IntFilterBuilder() : base(FieldType.Int, FieldType.IntArray) { }

        protected override Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = LongConverter.Convert(filter.RangeFrom ?? long.MinValue);
            var to = LongConverter.Convert(filter.RangeTo ?? long.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = LongConverter.Convert(filter.SingleValue);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    class TextAndLiteralFilterBuilder : TypedFilterBuilderBase
    {
        public TextAndLiteralFilterBuilder() : base(FieldType.Text, FieldType.TextArray, FieldType.Literal, FieldType.LiteralArray) { }

        protected override Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForTextAndLiteralFieldsException(field.Name);
        }

        protected override Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = StringConverter.Convert(filter.SingleValue);
            var terms = ExtractTerms(field.Name, value, perFieldAnalyzer);
            return new FieldCacheTermsFilter(field.Name, terms);
        }

        string[] ExtractTerms(string fieldName, string value, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            TokenStream tokenStream = null;

            try
            {
                tokenStream = perFieldAnalyzer.GetTokenStream(fieldName, value);

                var offsetAttribute = new OffsetAttribute();
                var charTermAttribute = new CharTermAttribute();
                var terms = new List<string>();

                tokenStream.AddAttributeImpl(offsetAttribute);
                tokenStream.AddAttributeImpl(charTermAttribute);
                tokenStream.Reset();

                while (tokenStream.IncrementToken())
                {
                    int start = offsetAttribute.StartOffset;
                    int end = offsetAttribute.EndOffset;
                    terms.Add(charTermAttribute.ToString());
                }

                return terms.ToArray();
            }
            finally
            {
                if (tokenStream != null)
                    IOUtils.DisposeWhileHandlingException(tokenStream);
            }
        }
    }

    #region Abstractions

    interface ITypedFilterBuilder
    {
        Filter Build(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);
    }

    abstract class TypedFilterBuilderBase : ITypedFilterBuilder
    {
        readonly FieldType[] types;

        public TypedFilterBuilderBase(params FieldType[] validForTypes)
        {
            types = validForTypes ?? throw new ArgumentNullException(nameof(validForTypes));
        }

        public Filter Build(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (filter == null)
                return null;

            if (!IsValidFieldType(field))
                throw new InvalidFilterTypeForFilterBuilderException(GetType(), field.Type);

            switch (filter.FilterType)
            {
                case QueryFilterType.SingleValue:
                    return filter.SingleValue == null ? null : BuildForSingleValue(request, filter, field, perFieldAnalyzer);

                case QueryFilterType.Range:
                    return filter.RangeFrom == null && filter.RangeTo == null ? null : BuildForRange(request, filter, field, perFieldAnalyzer);

                default:
                    throw new UnknownQueryFilterTypeException(filter.FilterType);
            }
        }

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);

        protected abstract Filter BuildForSingleValue(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);

        protected abstract Filter BuildForRange(ISearchRequest request, IQueryFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);
    }

    #endregion
}
