using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface ISuggestion
    {
        string Text { get; }

        int Weight { get; }

        IDictionary<string, object> ExtraData { get; }
    }
}
