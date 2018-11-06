namespace SmartSearch.Abstractions
{
    public interface ISearchRequest
    {
        string DomainName { get; }

        string Query { get; }

        ISortOption[] SortOptions { get; }
    }
}
