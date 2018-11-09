namespace SmartSearch.Abstractions
{
    public interface ISuggestionIndexService
    {
        void CreateIndex(ISuggestionProvider suggestionProvider);
    }
}
