using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchRequest : ISearchRequest
    {
        public string Query { get; set; }

        public IQueryFilter[] Filters { get; set; } = new IQueryFilter[0];

        public ISortOption[] SortOptions { get; set; } = new ISortOption[0];

        public int StartIndex { get; set; }

        public int PageSize { get; set; } = 100;
    }
}
