using System.Collections.ObjectModel;

namespace SmartSearch.Abstractions
{
    public interface ISearchDomain
    {
        string Name { get; }
        ReadOnlyCollection<IField> Fields { get; }
        IAnalysisSettings AnalysisSettings { get; }
    }
}