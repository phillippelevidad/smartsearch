using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchResult : ISearchResult
    {
        public IDocument[] Documents { get; set; }

        public int TotalCount { get; set; }

        public SearchResult()
        {
            Documents = new IDocument[0];
        }

        public SearchResult(IDocument[] results, int totalCount)
        {
            Documents = results;
            TotalCount = totalCount;
        }
    }
}
