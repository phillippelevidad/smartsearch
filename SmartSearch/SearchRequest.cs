using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchRequest : ISearchRequest
    {
        public string Query { get; set; }

        public int StartIndex { get; set; }

        public int PageSize { get; set; }

        public ISortOption[] SortOptions { get; set; }
    }
}
