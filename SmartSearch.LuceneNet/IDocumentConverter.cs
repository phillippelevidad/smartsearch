using Lucene.Net.Documents;
using SmartSearch.Abstractions;
using System;
using LuceneDocument = Lucene.Net.Documents.Document;
using LuceneField = Lucene.Net.Documents.Field;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet
{
    public interface IDocumentConverter
    {
        LuceneDocument Convert(IDomain domain, IDocument sourceDocument);
    }

    public class DefaultDocumentConverter : IDocumentConverter
    {
        readonly IFieldConverter fieldConverter;

        public DefaultDocumentConverter() : this(new DefaultFieldConverter())
        {
        }

        public DefaultDocumentConverter(IFieldConverter fieldConverter)
        {
            this.fieldConverter = fieldConverter;
        }

        public LuceneDocument Convert(IDomain domain, IDocument sourceDocument)
        {
            var luceneDocument = new LuceneDocument();

            foreach (var field in domain.Fields)
            {
                if (IsArrayField(field))
                    HandleArrayField(field, sourceDocument, luceneDocument);

                else
                    HandleCommonField(field, sourceDocument, luceneDocument);
            }

            return luceneDocument;
        }

        bool IsArrayField(IField field)
        {
            switch (field.Type)
            {
                case SourceFieldType.DateArray:
                case SourceFieldType.DoubleArray:
                case SourceFieldType.IntArray:
                case SourceFieldType.LiteralArray:
                case SourceFieldType.TextArray:
                    return true;
                default:
                    return false;
            }
        }

        void HandleArrayField(IField field, IDocument sourceDocument, LuceneDocument luceneDocument)
        {
            var array = sourceDocument.Fields[field.Name] as Array;
            if (array == null) return;

            var length = array.GetLength(0);
            if (length == 0) return;

            for (int i = 0; i < length; i++)
            {
                var value = array.GetValue(i);
                var luceneField = fieldConverter.Convert(field, value);
                luceneDocument.Add(luceneField);
            }
        }

        void HandleCommonField(IField field, IDocument sourceDocument, LuceneDocument luceneDocument)
        {
            var value = sourceDocument.Fields[field.Name];
            if (value == null) return;

            var luceneField = fieldConverter.Convert(field, value);
            luceneDocument.Add(luceneField);
        }

        LuceneField ConvertField(IField field, object value)
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

                case SourceFieldType.Literal:
                case SourceFieldType.LiteralArray:
                    return new TextField(field.Name, (string)value, store);

                case SourceFieldType.Text:
                case SourceFieldType.TextArray:
                    return new TextField(field.Name, (string)value, store);

                default:
                case SourceFieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}