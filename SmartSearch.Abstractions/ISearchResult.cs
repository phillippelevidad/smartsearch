namespace SmartSearch.Abstractions
{
    public interface ISearchResult
    {
        int TotalCount { get; }

        IDocument[] Results { get; }
    }
}
