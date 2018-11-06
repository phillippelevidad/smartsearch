namespace SmartSearch.Abstractions
{
    public interface IIndexService
    {
        void CreateIndex(ISearchDomain domain, IDocumentProvider documentProvider);
    }
}
