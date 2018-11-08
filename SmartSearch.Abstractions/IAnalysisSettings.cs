using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface IAnalysisSettings
    {
        IEnumerable<string[]> Synonyms { get; }

        string[] Stopwords { get; }
    }
}
