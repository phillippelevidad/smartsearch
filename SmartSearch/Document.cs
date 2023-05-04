using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Id}")]
    public class Document : IDocument
    {
        public Document(string id, IDictionary<string, object> fields)
        {
            Id = id;
            Fields = new ReadOnlyDictionary<string, object>(fields ?? new Dictionary<string, object>());
        }

        public string Id { get; }
        public ReadOnlyDictionary<string, object> Fields { get; }
    }

    public class DocumentBuilder
    {
        private readonly string id;
        private readonly Dictionary<string, object> fields;

        public DocumentBuilder(string id, int initialFieldCapacity = 0)
        {
            this.id = id;
            fields = new Dictionary<string, object>(initialFieldCapacity);
        }

        public DocumentBuilder Add(string name, object value)
        {
            fields.Add(name, value);
            return this;
        }

        public DocumentBuilder Add(IEnumerable<KeyValuePair<string, object>> fields)
        {
            if (fields != null)
                foreach (var field in fields)
                    this.fields.Add(field.Key, field.Value);
            return this;
        }

        public Document Build() => new Document(id, fields);
    }
}