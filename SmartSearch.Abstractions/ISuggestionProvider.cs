using System;

namespace SmartSearch.Abstractions
{
    public interface ISuggestionProvider : IDisposable
    {
        ISuggestionReader GetSuggestionReader();
    }
}
