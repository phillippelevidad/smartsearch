using System.Collections.ObjectModel;

namespace SmartSearch.Abstractions
{
    public interface IAnalysisSettings
    {
        ReadOnlyCollection<string[]> Synonyms { get; }
        ReadOnlyCollection<string> Stopwords { get; }
    }
}