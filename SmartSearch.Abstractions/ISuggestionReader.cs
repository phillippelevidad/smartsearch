using System;

namespace SmartSearch.Abstractions
{
    public interface ISuggestionReader : IDisposable
    {
        ISuggestion CurrentSuggestion { get; }

        bool ReadNext();
    }
}
