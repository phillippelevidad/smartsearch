namespace SmartSearch.Abstractions
{
    public interface ISuggestionResult
    {
        ISuggestion[] Suggestions { get; }
    }
}
