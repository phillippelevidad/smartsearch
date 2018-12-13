using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Id}, Operation = {OperationType}")]
    public class DocumentOperation : Document, IDocumentOperation
    {
        public DocumentOperationType OperationType { get; set; }

        public DocumentOperation() : this(null, null)
        {
        }

        public DocumentOperation(string id) : this(id, DocumentOperationType.AddOrUpdate, null)
        {
        }

        public DocumentOperation(string id, DocumentOperationType operationType) : this(id, operationType, null)
        {
        }

        public DocumentOperation(string id, IDictionary<string, object> fields) : this(id, DocumentOperationType.AddOrUpdate, fields)
        {
        }

        public DocumentOperation(string id, DocumentOperationType operationType, IDictionary<string, object> fields)
        {
            Id = id;
            OperationType = operationType;
            Fields = fields ?? new Dictionary<string, object>();
        }
    }
}