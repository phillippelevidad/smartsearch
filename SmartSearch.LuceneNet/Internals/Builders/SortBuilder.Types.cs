using Lucene.Net.Search;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class LiteralSortBuilder : TypedSortBuilderBase
    {
        public LiteralSortBuilder() : base(FieldType.Literal, FieldType.LiteralArray) { }

        protected override SortField BuildTypedSortField(ISearchDomain domain, ISortOption sortOption, IField field)
        {
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(field.Name, SortFieldType.STRING, sortDescending);
        }
    }

    class NumericSortBuilder : TypedSortBuilderBase
    {
        public NumericSortBuilder() : base(
            FieldType.Bool, FieldType.BoolArray,
            FieldType.Date, FieldType.DateArray,
            FieldType.Double, FieldType.DoubleArray,
            FieldType.Int, FieldType.IntArray)
        { }

        protected override SortField BuildTypedSortField(ISearchDomain domain, ISortOption sortOption, IField field)
        {
            var type = GetSortFieldType(field);
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(field.Name, type, sortDescending);
        }

        SortFieldType GetSortFieldType(IField field)
        {
            switch (field.Type)
            {
                case FieldType.Date:
                case FieldType.DateArray:
                    return SortFieldType.INT64;

                case FieldType.Double:
                case FieldType.DoubleArray:
                    return SortFieldType.DOUBLE;

                case FieldType.Bool:
                case FieldType.BoolArray:
                case FieldType.Int:
                case FieldType.IntArray:
                    return SortFieldType.INT32;

                default:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }

    class TextSortBuilder : TypedSortBuilderBase
    {
        public TextSortBuilder() : base(FieldType.Text, FieldType.TextArray) { }

        protected override SortField BuildTypedSortField(ISearchDomain domain, ISortOption sortOption, IField field)
        {
            var sortableTextField = new SortableTextFieldSpecification().CreateFrom(field);
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(sortableTextField.Name, SortFieldType.STRING, sortDescending);
        }
    }

    #region Abstractions

    interface ITypedSortBuilder
    {
        SortField Build(ISearchDomain domain, ISortOption sortOption, IField field);
    }

    abstract class TypedSortBuilderBase : ITypedSortBuilder
    {
        readonly FieldType[] types;

        public TypedSortBuilderBase(params FieldType[] validForTypes)
        {
            types = validForTypes ?? throw new ArgumentNullException(nameof(validForTypes));
        }

        public SortField Build(ISearchDomain domain, ISortOption sortOption, IField field)
        {
            if (sortOption == null)
                return null;

            if (!IsValidFieldType(field))
                throw new InvalidFilterTypeForFilterBuilderException(GetType(), field.Type);

            return BuildTypedSortField(domain, sortOption, field);
        }

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);

        protected abstract SortField BuildTypedSortField(ISearchDomain domain, ISortOption sortOption, IField field);
    }

    #endregion
}
