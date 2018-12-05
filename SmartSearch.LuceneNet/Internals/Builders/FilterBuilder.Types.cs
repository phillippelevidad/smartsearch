using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Spatial.Prefix.Tree;
using Lucene.Net.Spatial.Queries;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.Converters;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using Spatial4n.Core.Context;
using Spatial4n.Core.Shapes;
using System;
using System.Linq;
using LuceneFilter = Lucene.Net.Search.Filter;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class BoolFilterBuilder : TypedFilterBuilderBase
    {
        public BoolFilterBuilder() : base(FieldType.Bool, FieldType.BoolArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = BoolConverter.ConvertToInt(filter.RangeFrom ?? false);
            var to = BoolConverter.ConvertToInt(filter.RangeTo ?? false);
            return NumericRangeFilter.NewInt32Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = BoolConverter.ConvertToInt(filter.SingleValue);
            return NumericRangeFilter.NewInt32Range(field.Name, value, value, true, true);
        }
    }

    class DateFilterBuilder : TypedFilterBuilderBase
    {
        public DateFilterBuilder() : base(FieldType.Date, FieldType.DateArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DateTimeConverter.ConvertToLong(filter.RangeFrom ?? DateTime.MinValue);
            var to = DateTimeConverter.ConvertToLong(filter.RangeTo ?? DateTime.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DateTimeConverter.ConvertToLong(filter.SingleValue);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    class DoubleFilterBuilder : TypedFilterBuilderBase
    {
        public DoubleFilterBuilder() : base(FieldType.Double, FieldType.DoubleArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DoubleConverter.Convert(filter.RangeFrom ?? double.MinValue);
            var to = DoubleConverter.Convert(filter.RangeTo ?? double.MaxValue);
            return NumericRangeFilter.NewDoubleRange(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DoubleConverter.Convert(filter.SingleValue);
            return NumericRangeFilter.NewDoubleRange(field.Name, value, value, true, true);
        }
    }

    class IntFilterBuilder : TypedFilterBuilderBase
    {
        public IntFilterBuilder() : base(FieldType.Int, FieldType.IntArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = LongConverter.Convert(filter.RangeFrom ?? long.MinValue);
            var to = LongConverter.Convert(filter.RangeTo ?? long.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = LongConverter.Convert(filter.SingleValue);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    class LatLngFilterBuilder : TypedFilterBuilderBase
    {
        readonly SpatialContext context;
        readonly GeohashPrefixTree grid;

        public LatLngFilterBuilder() : base(FieldType.LatLng)
        {
            context = SpatialFactory.CreateSpatialContext();
            grid = SpatialFactory.CreatePrefixTree();
        }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForLatLngFieldsException(field.Name);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (!(filter.SingleValue is ILatLngFilterValue geoFilter))
                throw new InvalidLatLngFilterValue(field.Name);

            var actionableCoordField = new ActionableLatLngFieldSpecification().CreateFrom(field);
            var points = geoFilter.Points.Select(p => context.MakePoint(p.Longitude, p.Latitude) as IShape).ToList();

            var args = new SpatialArgs(SpatialOperation.Intersects, context.MakeRectangle(
                geoFilter.Points.Min(p => p.Longitude),
                geoFilter.Points.Max(p => p.Longitude),
                geoFilter.Points.Min(p => p.Latitude),
                geoFilter.Points.Max(p => p.Latitude)));

            var strategy = SpatialFactory.CreatePrefixTreeStrategy(actionableCoordField.Name);
            var query = strategy.MakeQuery(args);
            return new QueryWrapperFilter(query);
        }
    }

    class LiteralFilterBuilder : TypedFilterBuilderBase
    {
        public LiteralFilterBuilder() : base(FieldType.Literal, FieldType.LiteralArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForTextAndLiteralFieldsException(field.Name);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = StringConverter.Convert(filter.SingleValue);
            var query = new TermQuery(new Term(filter.FieldName, value));
            return new QueryWrapperFilter(query);
        }
    }

    class TextFilterBuilder : TypedFilterBuilderBase
    {
        // https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#_reserved_characters
        static readonly string[] reservedCharacters = new string[] { "\\", "+", "-", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":", "/" };

        public TextFilterBuilder() : base(FieldType.Literal, FieldType.LiteralArray, FieldType.Text, FieldType.TextArray) { }

        protected override LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForTextAndLiteralFieldsException(field.Name);
        }

        protected override LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = EscapeReservedCharacters(StringConverter.Convert(filter.SingleValue));
            var filterExpression = $"+({value})";

            var parser = new QueryParser(Definitions.LuceneVersion, field.Name, perFieldAnalyzer);
            var query = parser.Parse(filterExpression);

            return new QueryWrapperFilter(query);
        }

        string EscapeReservedCharacters(string input)
        {
            foreach (var item in reservedCharacters)
                input = input.Replace(item, "\\" + item);

            return input;
        }
    }

    #region Abstractions

    interface ITypedFilterBuilder
    {
        LuceneFilter Build(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);
    }

    abstract class TypedFilterBuilderBase : ITypedFilterBuilder
    {
        readonly FieldType[] types;

        public TypedFilterBuilderBase(params FieldType[] validForTypes)
        {
            types = validForTypes ?? throw new ArgumentNullException(nameof(validForTypes));
        }

        public LuceneFilter Build(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (filter == null)
                return null;

            if (!IsValidFieldType(field))
                throw new InvalidFilterTypeForFilterBuilderException(GetType(), field.Type);

            switch (filter.FilterType)
            {
                case FilterType.SingleValue:
                    return filter.SingleValue == null ? null : BuildForSingleValue(request, filter, field, perFieldAnalyzer);

                case FilterType.Range:
                    return filter.RangeFrom == null && filter.RangeTo == null ? null : BuildForRange(request, filter, field, perFieldAnalyzer);

                default:
                    throw new UnknownQueryFilterTypeException(filter.FilterType);
            }
        }

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);

        protected abstract LuceneFilter BuildForSingleValue(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);

        protected abstract LuceneFilter BuildForRange(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);
    }

    #endregion
}
