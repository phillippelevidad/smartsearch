namespace SmartSearch.Abstractions
{
    public interface IIndexService
    {
        void CreateIndex(IDomain domain, IDocumentProvider documentProvider);
    }
}
