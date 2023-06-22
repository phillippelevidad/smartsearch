using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Spatial.Prefix.Tree;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using Spatial4n.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using FieldType = SmartSearch.Abstractions.FieldType;
using LuceneField = Lucene.Net.Documents.Field;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    internal interface ITypedIndexableFieldConverter
    {
        IEnumerable<IIndexableField> Convert(
            InternalSearchDomain domain,
            IField field,
            InternalDocument sourceDocument
        );
    }

    internal class BoolIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public BoolIndexableFieldConverter() : base(FieldType.Bool, FieldType.BoolArray) { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        ) =>
            new Int32Field(field.Name, BoolConverter.ConvertToInt(value), GetFieldStore(field))
            {
                Boost = field.RelevanceModifier
            };
    }

    internal class DateIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public DateIndexableFieldConverter() : base(FieldType.Date, FieldType.DateArray) { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        ) =>
            new Int64Field(field.Name, DateTimeConverter.ConvertToLong(value), GetFieldStore(field))
            {
                Boost = field.RelevanceModifier
            };
    }

    internal class DoubleIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public DoubleIndexableFieldConverter() : base(FieldType.Double, FieldType.DoubleArray) { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        ) =>
            new DoubleField(field.Name, DoubleConverter.Convert(value), GetFieldStore(field))
            {
                Boost = field.RelevanceModifier
            };
    }

    internal class IntIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public IntIndexableFieldConverter() : base(FieldType.Int, FieldType.IntArray) { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        ) =>
            new Int64Field(field.Name, LongConverter.Convert(value), GetFieldStore(field))
            {
                Boost = field.RelevanceModifier
            };
    }

    internal class LatLngIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        private readonly SpatialContext context;
        private readonly GeohashPrefixTree grid;

        public LatLngIndexableFieldConverter() : base(FieldType.LatLng)
        {
            context = SpatialFactory.CreateSpatialContext();
            grid = SpatialFactory.CreatePrefixTree();
        }

        public override IEnumerable<IIndexableField> Convert(
            InternalSearchDomain domain,
            IField field,
            InternalDocument sourceDocument
        )
        {
            if (sourceDocument.Fields[field.Name] == null)
                return new IIndexableField[0];

            if (!(sourceDocument.Fields[field.Name] is ILatLng coordinate))
                throw new LatLngFieldValueMustImplementIGeoCoordinateException(field.Name);

            var strategy = SpatialFactory.CreatePrefixTreeStrategy(field.Name);

            var value =
                PrepareValueIfFieldIsSpecialized(domain, field, sourceDocument.Fields[field.Name])
                as ILatLng;
            var lat = value.Latitude;
            var lng = value.Longitude;

            var store = GetFieldStore(field);
            var point = context.MakePoint(lng, lat);

            if (store == LuceneField.Store.YES)
                return new IIndexableField[]
                {
                    new StoredField(field.Name, value.ToWellKnownText())
                };

            return strategy.CreateIndexableFields(point);
        }
    }

    internal class LiteralIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public LiteralIndexableFieldConverter() : base(FieldType.Literal, FieldType.LiteralArray)
        { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        )
        {
            var stringValue = StringConverter.Convert(value);

            // Ugly hack, but this is now the best way to id a field built for sorting.
            // See Internals/SpecializedFields/SortableTextField.cs
            var isForSorting = field.Name.EndsWith("_srt");
            if (isForSorting)
            {
                return new SortedDocValuesField(
                    field.Name,
                    new Lucene.Net.Util.BytesRef(stringValue)
                );
            }
            else
            {
                var store = GetFieldStore(field);
                var luceneField = new TextField(field.Name, stringValue, store);
                luceneField.Boost = field.RelevanceModifier;
                return luceneField;
            }
        }
    }

    internal class TextIndexableFieldConverter : TypedSimpleOrArrayIndexableFieldConverterBase
    {
        public TextIndexableFieldConverter() : base(FieldType.Text, FieldType.TextArray) { }

        protected override IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        ) =>
            new TextField(field.Name, StringConverter.Convert(value), GetFieldStore(field))
            {
                Boost = field.RelevanceModifier
            };
    }

    #region Base classes

    internal abstract class TypedIndexableFieldConverterBase : ITypedIndexableFieldConverter
    {
        private readonly FieldType[] types;

        public TypedIndexableFieldConverterBase(params FieldType[] validForTypes)
        {
            types = validForTypes ?? throw new ArgumentNullException(nameof(validForTypes));
        }

        public abstract IEnumerable<IIndexableField> Convert(
            InternalSearchDomain domain,
            IField field,
            InternalDocument sourceDocument
        );

        protected virtual LuceneField.Store GetFieldStore(IField field)
        {
            // NO = analyzed fields, best for searching
            // YES = not-analyzed fields, only exact matches are returned

            if (!(field is ISpecializedField specialized))
                return LuceneField.Store.YES;

            return specialized.AnalyzeField ? LuceneField.Store.NO : LuceneField.Store.YES;
        }

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);

        protected virtual object PrepareValueIfFieldIsSpecialized(
            InternalSearchDomain domain,
            IField field,
            object value
        )
        {
            if (field is ISpecializedField specialized)
                value = specialized.PrepareFieldValueForIndexing(value);

            return value;
        }
    }

    internal abstract class TypedSimpleOrArrayIndexableFieldConverterBase
        : TypedIndexableFieldConverterBase
    {
        public TypedSimpleOrArrayIndexableFieldConverterBase(params FieldType[] validForTypes)
            : base(validForTypes) { }

        public override IEnumerable<IIndexableField> Convert(
            InternalSearchDomain domain,
            IField field,
            InternalDocument sourceDocument
        )
        {
            if (field.IsArray())
                foreach (var f in ConvertArrayField(domain, field, sourceDocument))
                    yield return f;
            else
            {
                var indexField = ConvertSimpleField(domain, field, sourceDocument);
                if (indexField != null)
                    yield return indexField;
            }
        }

        protected virtual IIndexableField[] ConvertArrayField(
            InternalSearchDomain domain,
            IField field,
            InternalDocument document
        )
        {
            if (
                !document.Fields.ContainsKey(field.Name)
                || !(document.Fields[field.Name] is Array array)
                || array.Length == 0
            )
            {
                return new IIndexableField[0];
            }

            // Ugly hack, but this is now the best way to id a field built for sorting.
            // See Internals/SpecializedFields/SortableTextField.cs
            var isForSorting = field.Name.EndsWith("_srt");
            if (isForSorting)
            {
                // A DocValuesField cannot be multi-valued, so we need to create one for each value.
                array = new string[] { string.Join(" ", array.OfType<string>()) };
            }

            var indexFields = new IIndexableField[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                var value = PrepareValueIfFieldIsSpecialized(domain, field, array.GetValue(i));
                indexFields[i] = GetIndexableField(domain, field, value);
            }

            return indexFields;
        }

        protected virtual IIndexableField ConvertSimpleField(
            InternalSearchDomain domain,
            IField field,
            InternalDocument document
        )
        {
            var value = PrepareValueIfFieldIsSpecialized(
                domain,
                field,
                document.Fields[field.Name]
            );
            return value == null ? null : GetIndexableField(domain, field, value);
        }

        protected abstract IIndexableField GetIndexableField(
            InternalSearchDomain domain,
            IField field,
            object value
        );
    }

    #endregion Base classes
}
