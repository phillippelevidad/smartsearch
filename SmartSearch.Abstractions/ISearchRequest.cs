namespace SmartSearch.Abstractions
{
    public interface ISearchRequest
    {
        string Query { get; }

        IQueryFilter[] Filters { get; }

        ISortOption[] SortOptions { get; }

        int StartIndex { get; }

        int PageSize { get; }
    }
}
