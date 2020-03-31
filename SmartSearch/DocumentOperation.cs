using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Id}, Operation = {OperationType}")]
    public class DocumentOperation : Document, IDocumentOperation
    {
        private DocumentOperation(string id, DocumentOperationType operationType, IDictionary<string, object> fields) : base(id, fields)
        {
            OperationType = operationType;
        }

        public DocumentOperationType OperationType { get; }

        public static DocumentOperation AddOrUpdate(string id, IDictionary<string, object> fields) => new DocumentOperation(id, DocumentOperationType.AddOrUpdate, fields);
        public static DocumentOperation Delete(string id) => new DocumentOperation(id, DocumentOperationType.Delete, null);
    }
}