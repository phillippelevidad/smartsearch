namespace SmartSearch.Abstractions
{
    public interface ISearchService
    {
        object Search(ISearchRequest request);
    }
}
