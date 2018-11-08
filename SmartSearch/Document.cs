using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Id}")]
    public class Document : IDocument
    {
        public string Id { get; set; }

        public IDictionary<string, object> Fields { get; set; }

        public Document()
        {
            Fields = new Dictionary<string, object>();
        }

        public Document(string id)
        {
            Id = id;
            Fields = new Dictionary<string, object>();
        }

        public Document(string id, IDictionary<string, object> fields)
        {
            Id = id;
            Fields = fields;
        }
    }
}
