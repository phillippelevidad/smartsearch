namespace SmartSearch.Abstractions
{
    public interface IIndexService
    {
        void CreateIndex(IIndexContext context, ISearchDomain domain, IDocumentProvider documentProvider);
    }
}
