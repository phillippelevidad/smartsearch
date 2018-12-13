using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchResult : ISearchResult
    {
        public IDocument[] Documents { get; set; }
        public IFacet[] Facets { get; set; }
        public int TotalCount { get; set; }

        public SearchResult() : this(null, null, 0)
        {
        }

        public SearchResult(IDocument[] documents, IFacet[] facets, int totalCount)
        {
            Documents = documents ?? new IDocument[0];
            Facets = facets ?? new IFacet[0];
            TotalCount = totalCount;
        }
    }
}