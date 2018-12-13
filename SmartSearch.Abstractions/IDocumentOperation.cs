namespace SmartSearch.Abstractions
{
    public enum DocumentOperationType
    {
        AddOrUpdate, Delete
    }

    public interface IDocumentOperation : IDocument
    {
        DocumentOperationType OperationType { get; }
    }
}