using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchResult : ISearchResult
    {
        public IDocument[] Results { get; set; }

        public int TotalCount { get; set; }

        public SearchResult()
        {
            Results = new IDocument[0];
        }

        public SearchResult(IDocument[] results, int totalCount)
        {
            Results = results;
            TotalCount = totalCount;
        }
    }
}
