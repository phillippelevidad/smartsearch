using Lucene.Net.Documents;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System;
using System.Collections.Generic;
using LuceneDocument = Lucene.Net.Documents.Document;
using LuceneField = Lucene.Net.Documents.Field;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    partial class DefaultDocumentConverter : IDocumentConverter
    {
        public LuceneDocument Convert(InternalSearchDomain domain, InternalDocument sourceDocument)
        {
            var luceneDocument = new LuceneDocument();

            luceneDocument.AddStringField(
                Definitions.DocumentIdFieldName, sourceDocument.Id, LuceneField.Store.YES);

            foreach (var indexField in GetIndexFields(domain, sourceDocument))
                luceneDocument.Add(indexField);

            return luceneDocument;
        }

        IEnumerable<IIndexableField> GetIndexFields(InternalSearchDomain domain, InternalDocument sourceDocument)
        {
            foreach (var field in domain.AllFields)
            {
                if (field.IsArray())
                    foreach (var f in ConvertArrayField(field, sourceDocument))
                        yield return f;

                else
                {
                    var indexField = ConvertSimpleField(field, sourceDocument);

                    if (indexField != null)
                        yield return indexField;
                }
            }
        }

        IIndexableField[] ConvertArrayField(IField field, InternalDocument sourceDocument)
        {
            if (!sourceDocument.Fields.ContainsKey(field.Name) ||
                !(sourceDocument.Fields[field.Name] is Array array) || array.Length == 0)
                return new IIndexableField[0];

            var indexFields = new IIndexableField[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                var value = array.GetValue(i);
                indexFields[i] = ConvertFieldInternal(field, value);
            }

            return indexFields;
        }

        IIndexableField ConvertSimpleField(IField field, InternalDocument sourceDocument)
        {
            var value = sourceDocument.Fields[field.Name];
            return value == null ? null : ConvertFieldInternal(field, value);
        }

        IIndexableField ConvertFieldInternal(IField field, object value)
        {
            var store = field is ISpecializedField
                ? LuceneField.Store.NO /* analyzed fields, built for searching, cannot be returned */
                : LuceneField.Store.YES /* not-analyzed fields, only exact matches, can be returned */;

            switch (field.Type)
            {
                case SourceFieldType.Bool:
                case SourceFieldType.BoolArray:
                    return new Int32Field(field.Name, BoolConverter.ConvertToInt(value), store) { Boost = GetFieldBoost(field) };

                case SourceFieldType.Date:
                case SourceFieldType.DateArray:
                    return new Int64Field(field.Name, DateTimeConverter.ConvertToLong(value), store) { Boost = GetFieldBoost(field) };

                case SourceFieldType.Double:
                case SourceFieldType.DoubleArray:
                    return new DoubleField(field.Name, DoubleConverter.Convert(value), store) { Boost = GetFieldBoost(field) };

                case SourceFieldType.Int:
                case SourceFieldType.IntArray:
                    return new Int64Field(field.Name, LongConverter.Convert(value), store) { Boost = GetFieldBoost(field) };

                case SourceFieldType.Text:
                case SourceFieldType.TextArray:
                    return new TextField(field.Name, StringConverter.Convert(value), store) { Boost = GetFieldBoost(field) };

                case SourceFieldType.Literal:
                case SourceFieldType.LiteralArray:
                    return new StringField(field.Name, StringConverter.Convert(value), store) { Boost = GetFieldBoost(field) };

                default:
                case SourceFieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }

        float GetFieldBoost(IField field)
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
    }
}