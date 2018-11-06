using Lucene.Net.Documents;
using SmartSearch.Abstractions;
using System;
using LuceneField = Lucene.Net.Documents.Field;

namespace SmartSearch.LuceneNet
{
    public interface IFieldConverter
    {
        LuceneField Convert(IField field, object value, int? arrayIndex = null);
    }

    public class DefaultFieldConverter : IFieldConverter
    {
        public LuceneField Convert(IField field, object value, int? arrayIndex = null)
        {
            var store = field.EnableReturning ? LuceneField.Store.YES : LuceneField.Store.NO;

            switch (field.Type)
            {
                case Abstractions.FieldType.Date:
                case Abstractions.FieldType.DateArray:
                    return new Int64Field(field.Name, ((DateTime)value).Ticks, store);

                case Abstractions.FieldType.Double:
                case Abstractions.FieldType.DoubleArray:
                    return new DoubleField(field.Name, (double)value, store);

                case Abstractions.FieldType.Int:
                case Abstractions.FieldType.IntArray:
                    return new Int64Field(field.Name, (long)value, store);

                case Abstractions.FieldType.Literal:
                case Abstractions.FieldType.LiteralArray:
                    return new TextField(field.Name, (string)value, store);

                case Abstractions.FieldType.Text:
                case Abstractions.FieldType.TextArray:
                    return new TextField(field.Name, (string)value, store);

                default:
                case Abstractions.FieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}