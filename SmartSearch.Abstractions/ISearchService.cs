namespace SmartSearch.Abstractions
{
    public interface ISearchService
    {
        ISearchResult Search(ISearchDomain domain, ISearchRequest request);
    }
}
