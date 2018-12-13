using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchRequest : ISearchRequest
    {
        public IFilter[] Filters { get; set; } = new IFilter[0];
        public int PageSize { get; set; } = 100;
        public string Query { get; set; }
        public ISortOption[] SortOptions { get; set; } = new ISortOption[0];
        public int StartIndex { get; set; }
    }
}