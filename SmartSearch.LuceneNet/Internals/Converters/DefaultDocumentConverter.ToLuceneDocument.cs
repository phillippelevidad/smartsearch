using Lucene.Net.Documents;
using Lucene.Net.Index;
using SmartSearch.Abstractions;
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

        IEnumerable<IIndexableField> GetIndexFields(InternalSearchDomain domain, InternalDocument document)
        {
            foreach (var field in domain.AllFields)
                foreach (var f in GetIndexFields(domain, document, field))
                    yield return f;
        }

        IEnumerable<IIndexableField> GetIndexFields(InternalSearchDomain domain, InternalDocument document, IField field)
        {
            switch (field.Type)
            {
                case SourceFieldType.Bool:
                case SourceFieldType.BoolArray:
                    return new BoolIndexableFieldConverter().Convert(domain, field, document);

                case SourceFieldType.Date:
                case SourceFieldType.DateArray:
                    return new DateIndexableFieldConverter().Convert(domain, field, document);

                case SourceFieldType.Double:
                case SourceFieldType.DoubleArray:
                    return new DoubleIndexableFieldConverter().Convert(domain, field, document);

                case SourceFieldType.Int:
                case SourceFieldType.IntArray:
                    return new IntIndexableFieldConverter().Convert(domain, field, document);

                case SourceFieldType.Text:
                case SourceFieldType.TextArray:
                    return new TextIndexableFieldConverter().Convert(domain, field, document);

                case SourceFieldType.Literal:
                case SourceFieldType.LiteralArray:
                    return new LiteralIndexableFieldConverter().Convert(domain, field, document);

                default:
                case SourceFieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}