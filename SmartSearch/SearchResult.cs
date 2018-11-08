using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class SearchResult : ISearchResult
    {
        public IDocument[] Documents { get; set; }

        public IFacet[] Facets { get; set; }

        public int TotalCount { get; set; }

        public SearchResult()
        {
            Documents = new IDocument[0];
            Facets = new IFacet[0];
        }

        public SearchResult(IDocument[] documents, IFacet[] facets, int totalCount)
        {
            Documents = documents;
            Facets = facets;
            TotalCount = totalCount;
        }
    }
}
