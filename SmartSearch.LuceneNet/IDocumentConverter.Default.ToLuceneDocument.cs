using Lucene.Net.Documents;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using LuceneDocument = Lucene.Net.Documents.Document;
using LuceneField = Lucene.Net.Documents.Field;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet
{
    public partial class DefaultDocumentConverter : IDocumentConverter
    {
        public LuceneDocument Convert(ISearchDomain domain, IDocument sourceDocument)
        {
            var luceneDocument = new LuceneDocument();

            luceneDocument.AddTextField(
                Definitions.DocumentIdFieldName, sourceDocument.Id, LuceneField.Store.YES);

            foreach (var indexField in GetIndexFields(domain, sourceDocument))
                luceneDocument.Add(indexField);

            return luceneDocument;
        }

        IEnumerable<IIndexableField> GetIndexFields(ISearchDomain domain, IDocument sourceDocument)
        {
            foreach (var field in domain.Fields)
            {
                if (IsArrayField(field))
                {
                    foreach (var f in ConvertArrayField(field, sourceDocument))
                        yield return f;
                }
                else
                {
                    var indexField = ConvertSimpleField(field, sourceDocument);

                    if (indexField != null)
                        yield return indexField;
                }
            }
        }

        IIndexableField[] ConvertArrayField(IField field, IDocument sourceDocument)
        {
            var array = sourceDocument.Fields[field.Name] as Array;
            if (array == null || array.Length == 0)
                return new IIndexableField[0];

            var indexFields = new IIndexableField[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                var value = array.GetValue(i);
                indexFields[i] = ConvertFieldInternal(field, value);
            }

            return indexFields;
        }

        IIndexableField ConvertSimpleField(IField field, IDocument sourceDocument)
        {
            var value = sourceDocument.Fields[field.Name];
            return value == null ? null : ConvertFieldInternal(field, value);
        }

        IIndexableField ConvertFieldInternal(IField field, object value)
        {
            var store = field.EnableReturning ? LuceneField.Store.YES : LuceneField.Store.NO;

            switch (field.Type)
            {
                case SourceFieldType.Date:
                case SourceFieldType.DateArray:
                    return new Int64Field(field.Name, ((DateTime)value).Ticks, store);

                case SourceFieldType.Double:
                case SourceFieldType.DoubleArray:
                    return new DoubleField(field.Name, (double)value, store);

                case SourceFieldType.Int:
                case SourceFieldType.IntArray:
                    return new Int64Field(field.Name, (long)value, store);

                case SourceFieldType.Text:
                case SourceFieldType.Literal:
                case SourceFieldType.TextArray:
                case SourceFieldType.LiteralArray:
                    return new TextField(field.Name, (string)value, store);

                default:
                case SourceFieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}