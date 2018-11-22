using Lucene.Net.Documents;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System;
using System.Collections.Generic;
using System.Linq;
using FieldType = SmartSearch.Abstractions.FieldType;
using LuceneField = Lucene.Net.Documents.Field;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    class BoolIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public BoolIndexableFieldConverter() : base(FieldType.Bool, FieldType.BoolArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value) =>
            new Int32Field(field.Name, BoolConverter.ConvertToInt(value), GetFieldStore(field)) { Boost = GetFieldBoost(field) };
    }

    class DateIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public DateIndexableFieldConverter() : base(FieldType.Date, FieldType.DateArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value) =>
            new Int64Field(field.Name, DateTimeConverter.ConvertToLong(value), GetFieldStore(field)) { Boost = GetFieldBoost(field) };
    }

    class DoubleIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public DoubleIndexableFieldConverter() : base(FieldType.Double, FieldType.DoubleArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value) =>
            new DoubleField(field.Name, DoubleConverter.Convert(value), GetFieldStore(field)) { Boost = GetFieldBoost(field) };
    }

    class IntIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public IntIndexableFieldConverter() : base(FieldType.Int, FieldType.IntArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value) =>
            new Int64Field(field.Name, LongConverter.Convert(value), GetFieldStore(field)) { Boost = GetFieldBoost(field) };
    }

    class LiteralIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public LiteralIndexableFieldConverter() : base(FieldType.Literal, FieldType.LiteralArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value)
        {
            var store = GetFieldStore(field);
            return store == LuceneField.Store.YES
                ? new StringField(field.Name, StringConverter.Convert(value), store)
                : new StringField(field.Name, StringConverter.Convert(value), store) { Boost = GetFieldBoost(field) };
        }
    }

    class TextIndexableFieldConverter : TypedIndexableFieldConverterBase
    {
        public TextIndexableFieldConverter() : base(FieldType.Text, FieldType.TextArray) { }

        protected override IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value) =>
            new TextField(field.Name, StringConverter.Convert(value), GetFieldStore(field)) { Boost = GetFieldBoost(field) };
    }

    interface ITypedIndexableFieldConverter
    {
        IEnumerable<IIndexableField> Convert(InternalSearchDomain domain, IField field, InternalDocument sourceDocument);
    }

    abstract class TypedIndexableFieldConverterBase : ITypedIndexableFieldConverter
    {
        readonly FieldType[] types;

        public TypedIndexableFieldConverterBase(params FieldType[] validForTypes)
        {
            types = validForTypes ?? throw new ArgumentNullException(nameof(validForTypes));
        }

        public IEnumerable<IIndexableField> Convert(InternalSearchDomain domain, IField field, InternalDocument sourceDocument)
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

        protected virtual bool IsValidFieldType(IField field) => types.Any(t => t == field.Type);

        protected virtual IIndexableField[] ConvertArrayField(InternalSearchDomain domain, IField field, InternalDocument document)
        {
            if (!document.Fields.ContainsKey(field.Name) ||
                !(document.Fields[field.Name] is Array array) || array.Length == 0)
                return new IIndexableField[0];

            var indexFields = new IIndexableField[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                var value = PrepareValueIfFieldIsSpecialized(domain, field, array.GetValue(i));
                indexFields[i] = GetIndexableField(domain, field, value);
            }

            return indexFields;
        }

        protected virtual IIndexableField ConvertSimpleField(InternalSearchDomain domain, IField field, InternalDocument document)
        {
            var value = PrepareValueIfFieldIsSpecialized(domain, field, document.Fields[field.Name]);
            return value == null ? null : GetIndexableField(domain, field, value);
        }

        protected virtual object PrepareValueIfFieldIsSpecialized(InternalSearchDomain domain, IField field, object value)
        {
            if (field is ISpecializedField specialized)
                value = specialized.PrepareFieldValueForIndexing(value);

            return value;
        }

        protected LuceneField.Store GetFieldStore(IField field)
        {
            // NO = analyzed fields, best for searching
            // YES = not-analyzed fields, only exact matches, can be returned

            if (!(field is ISpecializedField specialized))
                return LuceneField.Store.YES;

            return specialized.AnalyzeField
                ? LuceneField.Store.NO : LuceneField.Store.YES;
        }

        protected float GetFieldBoost(IField field)
        {
            switch (field.Relevance)
            {
                case FieldRelevance.Normal:
                    return 1f;

                case FieldRelevance.High:
                    return 2f;

                case FieldRelevance.Higher:
                    return 4f;

                default:
                    throw new UnknownFieldRelevanceException(field.Relevance);
            }
        }

        protected abstract IIndexableField GetIndexableField(InternalSearchDomain domain, IField field, object value);
    }
}
