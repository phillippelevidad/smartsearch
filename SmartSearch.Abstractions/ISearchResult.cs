namespace SmartSearch.Abstractions
{
    public interface ISearchResult
    {
        IDocument[] Documents { get; }
        IFacet[] Facets { get; }
        int TotalCount { get; }
    }
}