namespace SmartSearch.Abstractions
{
    public interface ISearchResult
    {
        int TotalCount { get; }

        IDocument[] Documents { get; }

        IFacet[] Facets { get; }
    }
}
