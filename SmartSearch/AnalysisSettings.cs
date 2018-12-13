using SmartSearch.Abstractions;
using System.Collections.Generic;

namespace SmartSearch
{
    public class AnalysisSettings : IAnalysisSettings
    {
        public string[] Stopwords { get; set; }
        public IEnumerable<string[]> Synonyms { get; set; }

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