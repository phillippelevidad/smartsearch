using Lucene.Net.Search;
using Lucene.Net.Spatial.Prefix.Tree;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using Spatial4n.Core.Context;
using Spatial4n.Core.Distance;
using System;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    internal interface ITypedSortBuilder
    {
        SortField Build(ISearchDomain domain, ISortOption sortOption, IField field);
    }

    internal class LatLngSortBuilder : TypedSortBuilderBase
    {
        private readonly SpatialContext context;
        private readonly GeohashPrefixTree grid;
        private readonly IndexSearcher indexSearcher;

        public LatLngSortBuilder(IndexSearcher indexSearcher) : base(FieldType.LatLng)
        {
            context = SpatialFactory.CreateSpatialContext();
            grid = SpatialFactory.CreatePrefixTree();
            this.indexSearcher = indexSearcher;
        }

        protected override SortField BuildTypedSortField(
            ISearchDomain domain,
            ISortOption sortOption,
            IField field
        )
        {
            if (sortOption.Reference == null)
                throw new MissingLatLngReferenceForSortingException();

            if (!(sortOption.Reference is ILatLng latLngReference))
                throw new ArgumentException(
                    $"Invalid sort reference for field '{sortOption.FieldName}'. Provide an implementation of '{typeof(ILatLng).FullName}'."
                );

            var actionableCoordField = new ActionableLatLngFieldSpecification().CreateFrom(field);
            var strategy = SpatialFactory.CreatePrefixTreeStrategy(actionableCoordField.Name);

            var point = context.MakePoint(latLngReference.Longitude, latLngReference.Latitude);
            var valueSource = strategy.MakeDistanceValueSource(point, DistanceUtils.DEG_TO_KM);

            var sortField = valueSource.GetSortField(
                sortOption.Direction == SortDirection.Descending
            );
            return sortField.Rewrite(indexSearcher);
        }
    }

    internal class LiteralSortBuilder : TypedSortBuilderBase
    {
        public LiteralSortBuilder() : base(FieldType.Literal, FieldType.LiteralArray) { }

        protected override SortField BuildTypedSortField(
            ISearchDomain domain,
            ISortOption sortOption,
            IField field
        )
        {
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(field.Name, SortFieldType.STRING, sortDescending);
        }
    }

    internal class NumericSortBuilder : TypedSortBuilderBase
    {
        public NumericSortBuilder()
            : base(
                FieldType.Bool,
                FieldType.BoolArray,
                FieldType.Date,
                FieldType.DateArray,
                FieldType.Double,
                FieldType.DoubleArray,
                FieldType.Int,
                FieldType.IntArray
            ) { }

        protected override SortField BuildTypedSortField(
            ISearchDomain domain,
            ISortOption sortOption,
            IField field
        )
        {
            var type = GetSortFieldType(field);
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(field.Name, type, sortDescending);
        }

        private SortFieldType GetSortFieldType(IField field)
        {
            switch (field.Type)
            {
                case FieldType.Date:
                case FieldType.DateArray:
                case FieldType.Int:
                case FieldType.IntArray:
                    return SortFieldType.INT64;

                case FieldType.Double:
                case FieldType.DoubleArray:
                    return SortFieldType.DOUBLE;

                case FieldType.Bool:
                case FieldType.BoolArray:
                    return SortFieldType.INT32;

                default:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }

    internal class TextSortBuilder : TypedSortBuilderBase
    {
        public TextSortBuilder() : base(FieldType.Text, FieldType.TextArray) { }

        protected override SortField BuildTypedSortField(
            ISearchDomain domain,
            ISortOption sortOption,
            IField field
        )
        {
            var sortableTextField = new SortableTextFieldSpecification().CreateFrom(field);
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(sortableTextField.Name, SortFieldType.STRING, sortDescending);
        }
    }

    #region Abstractions

    internal abstract class TypedSortBuilderBase : ITypedSortBuilder
    {
        private readonly FieldType[] types;

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

        protected abstract SortField BuildTypedSortField(
            ISearchDomain domain,
            ISortOption sortOption,
            IField field
        );

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);
    }

    #endregion Abstractions
}
