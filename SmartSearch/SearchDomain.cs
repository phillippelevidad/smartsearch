using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartSearch
{
    public class SearchDomain : ISearchDomain
    {
        public string Name { get; }
        public ReadOnlyCollection<IField> Fields { get; }
        public IAnalysisSettings AnalysisSettings { get; }

        public SearchDomain(string name, IEnumerable<IField> fields, IAnalysisSettings analysisSettings = null)
        {
            Name = name;
            Fields = (fields ?? Array.Empty<IField>()).ToList().AsReadOnly();
            AnalysisSettings = analysisSettings;
        }
    }
}