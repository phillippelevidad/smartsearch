using SmartSearch.Abstractions;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Internals
{
    class InternalDocument : IDocument
    {
        public string Id { get; private set; }

        public IDictionary<string, object> Fields { get; private set; }

        InternalDocument(string id, IDictionary<string, object> fields)
        {
            Id = id;
            Fields = fields;
        }

        internal static InternalDocument CreateFrom(InternalSearchDomain domain, IDocument document)
        {
            var fields = new Dictionary<string, object>(domain.Fields.Length + domain.SpecializedFields.Length);

            foreach (var field in document.Fields)
                fields.Add(field.Key, field.Value);

            foreach (var field in domain.SpecializedFields)
            {
                if (!document.Fields.ContainsKey(field.OriginalName))
                    continue;

                var value = document.Fields[field.OriginalName];
                fields.Add(field.Name, value);
            }

            return new InternalDocument(document.Id, fields);
        }
    }
}
