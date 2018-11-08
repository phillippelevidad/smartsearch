using Lucene.Net.Facet;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    class FacetDocumentConverter : IDocumentConverter
    {
        public LuceneDocument Convert(InternalSearchDomain domain, InternalDocument sourceDocument)
        {
            var luceneDocument = new LuceneDocument();

            foreach (var facetField in GetFacetFields(domain, sourceDocument))
                luceneDocument.Add(facetField);

            return luceneDocument;
        }

        IEnumerable<FacetField> GetFacetFields(ISearchDomain domain, IDocument sourceDocument)
        {
            var fields = domain.GetFacetEnabledFields();

            foreach (var field in fields)
            {
                if (field.IsArray())
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

        FacetField[] ConvertArrayField(IField field, IDocument sourceDocument)
        {
            if (!(sourceDocument.Fields[field.Name] is Array array) || array.Length == 0)
                return new FacetField[0];

            var facetFields = new List<FacetField>(array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                var facet = ConvertFieldInternal(field, array.GetValue(i));

                if (facet != null)
                    facetFields.Add(facet);
            }

            return facetFields.ToArray();
        }

        FacetField ConvertSimpleField(IField field, IDocument sourceDocument)
        {
            var value = sourceDocument.Fields[field.Name];
            return ConvertFieldInternal(field, value);
        }

        FacetField ConvertFieldInternal(IField field, object value)
        {
            if (value == null)
                return null;

            return new FacetField(field.Name, value.ToString());
        }

        public IDocument Convert(InternalSearchDomain domain, LuceneDocument luceneDocument)
        {
            throw new InvalidOperationException("A facet document is not supposed to be converted back into a result document.");
        }
    }
}