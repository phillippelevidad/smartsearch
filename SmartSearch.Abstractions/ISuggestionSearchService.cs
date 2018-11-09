namespace SmartSearch.Abstractions
{
    public interface ISuggestionSearchService
    {
        ISuggestionResult Search(ISuggestionRequest request);
    }
}
