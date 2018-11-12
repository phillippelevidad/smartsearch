using System;

namespace SmartSearch.Abstractions
{
    // https://stackoverflow.com/questions/24968697/how-to-implement-auto-suggest-using-lucenes-new-analyzinginfixsuggester-api
    public interface ISuggestionReader : IDisposable
    {
        ISuggestion CurrentSuggestion { get; }

        bool ReadNext();
    }
}
