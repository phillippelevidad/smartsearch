using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartSearch.LuceneNet.Internals
{
    internal class InternalDocument : IDocument
    {
        public string Id { get; private set; }
        public ReadOnlyDictionary<string, object> Fields { get; private set; }

        private InternalDocument(string id, IDictionary<string, object> fields)
        {
            Id = id;
            Fields = new ReadOnlyDictionary<string, object>(fields);
        }

        internal static InternalDocument CreateFrom(InternalSearchDomain domain, IDocument document)
        {
            var fields = new Dictionary<string, object>(domain.Fields.Count + domain.SpecializedFields.Count);

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