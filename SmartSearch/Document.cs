using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Id}")]
    public class Document : IDocument
    {
        public IDictionary<string, object> Fields { get; set; }
        public string Id { get; set; }

        public Document() : this(null, null)
        {
        }

        public Document(string id) : this(id, null)
        {
        }

        public Document(string id, IDictionary<string, object> fields)
        {
            Id = id;
            Fields = fields ?? new Dictionary<string, object>();
        }
    }
}