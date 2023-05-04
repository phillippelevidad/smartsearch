using System.Collections.ObjectModel;

namespace SmartSearch.Abstractions
{
    public interface ISearchRequest
    {
        int StartIndex { get; }
        int PageSize { get; }
        string Query { get; }
        ReadOnlyCollection<ISortOption> SortOptions { get; }
        ReadOnlyCollection<IFilter> Filters { get; }
    }
}