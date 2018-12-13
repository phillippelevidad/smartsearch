using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface IAnalysisSettings
    {
        string[] Stopwords { get; }
        IEnumerable<string[]> Synonyms { get; }
    }
}