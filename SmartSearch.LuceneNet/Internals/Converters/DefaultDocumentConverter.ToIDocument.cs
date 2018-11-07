using Lucene.Net.Index;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.Helpers;
using System.Collections.Generic;
using LuceneDocument = Lucene.Net.Documents.Document;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    partial class DefaultDocumentConverter : IDocumentConverter
    {
        public IDocument Convert(ISearchDomain domain, LuceneDocument luceneDocument)
        {
            var result = new Document
            {
                Id = luceneDocument.Get(Definitions.DocumentIdFieldName),
                Fields = new Dictionary<string, object>(domain.Fields.Length)
            };

            foreach (var field in domain.Fields)
            {
                var value = ArrayFieldHelper.IsArrayField(field)
                    ? ParseArrayField(field, luceneDocument)
                    : ParseCommonField(field, luceneDocument);

                result.Fields.Add(field.Name, value);
            }

            return result;
        }

        object[] ParseArrayField(IField field, LuceneDocument luceneDocument)
        {
            var luceneFields = luceneDocument.GetFields(field.Name);
            var values = new object[luceneFields.Length];

            for (int i = 0; i < luceneFields.Length; i++)
                values[i] = ParseFieldInternal(field, luceneFields[i]);

            return values;
        }

        object ParseCommonField(IField field, LuceneDocument luceneDocument)
        {
            var luceneField = luceneDocument.GetField(field.Name);
            return luceneField == null ? null : ParseFieldInternal(field, luceneField);
        }

        object ParseFieldInternal(IField field, IIndexableField luceneField)
        {
            switch (field.Type)
            {
                case SourceFieldType.Date:
                case SourceFieldType.DateArray:
                    return luceneField.GetInt64Value();

                case SourceFieldType.Double:
                case SourceFieldType.DoubleArray:
                    return luceneField.GetDoubleValue();

                case SourceFieldType.Int:
                case SourceFieldType.IntArray:
                    return luceneField.GetInt64Value();

                case SourceFieldType.Text:
                case SourceFieldType.Literal:
                case SourceFieldType.TextArray:
                case SourceFieldType.LiteralArray:
                    return luceneField.GetStringValue();

                default:
                case SourceFieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}