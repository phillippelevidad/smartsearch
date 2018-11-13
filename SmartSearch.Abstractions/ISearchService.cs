namespace SmartSearch.Abstractions
{
    public interface ISearchService
    {
        ISearchResult Search(IIndexContext context, ISearchDomain domain, ISearchRequest request);
    }
}
