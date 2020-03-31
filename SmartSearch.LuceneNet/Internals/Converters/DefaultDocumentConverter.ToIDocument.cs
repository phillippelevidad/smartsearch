using Lucene.Net.Index;
using SmartSearch.Abstractions;
using System.Collections.Generic;
using LuceneDocument = Lucene.Net.Documents.Document;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    partial class DefaultDocumentConverter : IDocumentConverter
    {
        public IDocument Convert(InternalSearchDomain domain, LuceneDocument luceneDocument)
        {
            var result = new Document
            {
                Id = luceneDocument.Get(Definitions.DocumentIdFieldName),
                Fields = new Dictionary<string, object>(domain.Fields.Length)
            };

            foreach (var field in domain.Fields)
            {
                var value = field.IsArray()
                    ? ParseArrayField(field, luceneDocument)
                    : ParseSingleValuedField(field, luceneDocument);

                result.Fields.Add(field.Name, value);
            }

            return result;
        }

        private object[] ParseArrayField(IField field, LuceneDocument luceneDocument)
        {
            var luceneFields = luceneDocument.GetFields(field.Name);
            var values = new object[luceneFields.Length];

            for (int i = 0; i < luceneFields.Length; i++)
                values[i] = ParseFieldInternal(field, luceneFields[i]);

            return values;
        }

        private object ParseFieldInternal(IField field, IIndexableField luceneField)
        {
            switch (field.Type)
            {
                case SourceFieldType.Bool:
                case SourceFieldType.BoolArray:
                    return BoolConverter.ConvertFromInt(luceneField.GetInt32Value().Value);

                case SourceFieldType.Date:
                case SourceFieldType.DateArray:
                    return DateTimeConverter.ConvertFromLong(luceneField.GetInt64Value().Value);

                case SourceFieldType.Double:
                case SourceFieldType.DoubleArray:
                    return luceneField.GetDoubleValue().Value;

                case SourceFieldType.Int:
                case SourceFieldType.IntArray:
                    return luceneField.GetInt64Value().Value;

                case SourceFieldType.LatLng:
                    return LatLng.FromWellKnownText(luceneField.GetStringValue());

                case SourceFieldType.Literal:
                case SourceFieldType.LiteralArray:
                case SourceFieldType.Text:
                case SourceFieldType.TextArray:
                    return luceneField.GetStringValue();

                default:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }

        private object ParseSingleValuedField(IField field, LuceneDocument luceneDocument)
        {
            var luceneField = luceneDocument.GetField(field.Name);
            return luceneField == null ? null : ParseFieldInternal(field, luceneField);
        }
    }
}