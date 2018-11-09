namespace SmartSearch.Abstractions
{
    public interface ISuggestionRequest
    {
        string Query { get; }

        int MaxSuggestions { get; }
    }
}
