using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchRequest : ISearchRequest
    {
        public string Query { get; set; }

        public int StartIndex { get; set; }

        public int PageSize { get; set; } = 100;

        public ISortOption[] SortOptions { get; set; }
    }
}
