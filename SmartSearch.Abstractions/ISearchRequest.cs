namespace SmartSearch.Abstractions
{
    public interface ISearchRequest
    {
        string Query { get; }

        int StartIndex { get; }

        int PageSize { get; }

        ISortOption[] SortOptions { get; }
    }
}
