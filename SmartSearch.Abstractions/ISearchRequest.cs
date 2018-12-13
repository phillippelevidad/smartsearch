namespace SmartSearch.Abstractions
{
    public interface ISearchRequest
    {
        IFilter[] Filters { get; }
        int PageSize { get; }
        string Query { get; }
        ISortOption[] SortOptions { get; }

        int StartIndex { get; }
    }
}