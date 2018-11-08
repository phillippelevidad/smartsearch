﻿using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.SpecializedFields;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Internals
{
    class InternalDocument : IDocument
    {
        public string Id { get; private set; }

        public IDictionary<string, object> Fields { get; private set; }

        internal InternalDocument(string id)
        {
            Id = id;
        }

        public static InternalDocument CreateFrom(InternalSearchDomain domain, IDocument document)
        {
            var newDocument = new InternalDocument(document.Id);
            var newFields = new Dictionary<string, object>(domain.Fields.Length + domain.SpecializedFields.Length);

            foreach (var field in document.Fields)
                newFields.Add(field.Key, field.Value);

            foreach (var spec in SpecificationsList.All)
            {
                foreach (var field in domain.SpecializedFields)
                {
                    if (!spec.IsSatisfiedBy(field))
                        continue;

                    var originalValue = document.Fields[field.OriginalName];
                    var specializedValue = spec.ConvertValue(field, originalValue);
                    newFields.Add(field.Name, specializedValue);
                }
            }

            newDocument.Fields = newFields;
            return newDocument;
        }
    }
}
