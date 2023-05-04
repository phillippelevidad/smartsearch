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
    internal interface ITypedFilterBuilder
    {
        LuceneFilter Build(ISearchRequest request, IFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);
    }

    internal class BoolFilterBuilder : TypedFilterBuilderBase
    {
        public BoolFilterBuilder() : base(FieldType.Bool, FieldType.BoolArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = BoolConverter.ConvertToInt(filter.FromValue ?? false);
            var to = BoolConverter.ConvertToInt(filter.ToValue ?? false);
            return NumericRangeFilter.NewInt32Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = BoolConverter.ConvertToInt(filter.Value);
            return NumericRangeFilter.NewInt32Range(field.Name, value, value, true, true);
        }
    }

    internal class DateFilterBuilder : TypedFilterBuilderBase
    {
        public DateFilterBuilder() : base(FieldType.Date, FieldType.DateArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DateTimeConverter.ConvertToLong(filter.FromValue ?? DateTime.MinValue);
            var to = DateTimeConverter.ConvertToLong(filter.ToValue ?? DateTime.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DateTimeConverter.ConvertToLong(filter.Value);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    internal class DoubleFilterBuilder : TypedFilterBuilderBase
    {
        public DoubleFilterBuilder() : base(FieldType.Double, FieldType.DoubleArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = DoubleConverter.Convert(filter.FromValue ?? double.MinValue);
            var to = DoubleConverter.Convert(filter.ToValue ?? double.MaxValue);
            return NumericRangeFilter.NewDoubleRange(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = DoubleConverter.Convert(filter.Value);
            return NumericRangeFilter.NewDoubleRange(field.Name, value, value, true, true);
        }
    }

    internal class IntFilterBuilder : TypedFilterBuilderBase
    {
        public IntFilterBuilder() : base(FieldType.Int, FieldType.IntArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var from = LongConverter.Convert(filter.FromValue ?? long.MinValue);
            var to = LongConverter.Convert(filter.ToValue ?? long.MaxValue);
            return NumericRangeFilter.NewInt64Range(field.Name, from, to, true, true);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = LongConverter.Convert(filter.Value);
            return NumericRangeFilter.NewInt64Range(field.Name, value, value, true, true);
        }
    }

    internal class LatLngFilterBuilder : TypedFilterBuilderBase
    {
        private readonly SpatialContext context;
        private readonly GeohashPrefixTree grid;

        public LatLngFilterBuilder() : base(FieldType.LatLng)
        {
            context = SpatialFactory.CreateSpatialContext();
            grid = SpatialFactory.CreatePrefixTree();
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForLatLngFieldsException(field.Name);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (!(filter.Value is ILatLngFilterValue geoFilter))
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

    internal class LiteralFilterBuilder : TypedFilterBuilderBase
    {
        public LiteralFilterBuilder() : base(FieldType.Literal, FieldType.LiteralArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForTextAndLiteralFieldsException(field.Name);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = StringConverter.Convert(filter.Value);
            var query = new TermQuery(new Term(filter.FieldName, value));
            return new QueryWrapperFilter(query);
        }
    }

    internal class TextFilterBuilder : TypedFilterBuilderBase
    {
        // https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html#_reserved_characters
        private static readonly string[] reservedCharacters = new string[] { "\\", "+", "-", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "\"", "~", "*", "?", ":", "/" };

        public TextFilterBuilder() : base(FieldType.Literal, FieldType.LiteralArray, FieldType.Text, FieldType.TextArray)
        {
        }

        protected override LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            throw new RangeFilterNotSupportedForTextAndLiteralFieldsException(field.Name);
        }

        protected override LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = EscapeReservedCharacters(StringConverter.Convert(filter.Value));
            var filterExpression = $"+({value})";

            var parser = new QueryParser(Definitions.LuceneVersion, field.Name, perFieldAnalyzer);
            var query = parser.Parse(filterExpression);

            return new QueryWrapperFilter(query);
        }

        private string EscapeReservedCharacters(string input)
        {
            foreach (var item in reservedCharacters)
                input = input.Replace(item, "\\" + item);

            return input;
        }
    }

    #region Abstractions

    internal abstract class TypedFilterBuilderBase : ITypedFilterBuilder
    {
        private readonly FieldType[] types;

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

            if (filter is IValueFilter valueFilter)
                return valueFilter.Value == null ? null : BuildValueFilter(request, valueFilter, field, perFieldAnalyzer);

            if (filter is IRangeFilter rangeFilter)
                return rangeFilter.FromValue == null && rangeFilter.ToValue == null ? null : BuildRangeFilter(request, rangeFilter, field, perFieldAnalyzer);

            throw new NotImplementedException($"TypedFilterBuilderBase.Build: unknown IFilter type '{filter.GetType()}' in SmartSearch.LuceneNet.Internals.Builders.");
        }

        protected abstract LuceneFilter BuildRangeFilter(ISearchRequest request, IRangeFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);

        protected abstract LuceneFilter BuildValueFilter(ISearchRequest request, IValueFilter filter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer);

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);
    }

    #endregion Abstractions
}