namespace SmartSearch.Abstractions
{
    public interface IDocumentOperation : IDocument
    {
        DocumentOperationType OperationType { get; }
    }

    public enum DocumentOperationType
    {
        AddOrUpdate, Delete
    }
}
