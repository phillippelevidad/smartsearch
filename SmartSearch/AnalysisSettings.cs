using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartSearch
{
    public class AnalysisSettings : IAnalysisSettings
    {
        public ReadOnlyCollection<string[]> Synonyms { get; }
        public ReadOnlyCollection<string> Stopwords { get; }

        public AnalysisSettings() : this(null, null)
        {
        }

        public AnalysisSettings(IEnumerable<string[]> synonyms, string[] stopwords)
        {
            Synonyms = (synonyms ?? Array.Empty<string[]>()).ToList().AsReadOnly();
            Stopwords = (stopwords ?? Array.Empty<string>()).ToList().AsReadOnly();
        }
    }
}