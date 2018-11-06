using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface IAnalysisSettings
    {
        IDictionary<string, string[]> Aliases { get; }

        string[] Stopwords { get; }
    }
}
