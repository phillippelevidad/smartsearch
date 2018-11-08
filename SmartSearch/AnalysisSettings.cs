using System.Collections.Generic;
using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class AnalysisSettings : IAnalysisSettings
    {
        public IEnumerable<string[]> Synonyms { get; set; }

        public string[] Stopwords { get; set; }

        public AnalysisSettings() : this(null, null)
        {
        }

        public AnalysisSettings(IEnumerable<string[]> synonyms, string[] stopwords)
        {
            Synonyms = synonyms ?? new string[0][];
            Stopwords = stopwords ?? new string[0];
        }
    }
}
