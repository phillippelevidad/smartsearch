namespace SmartSearch.Abstractions
{
    public interface ISearchDomain
    {
        string Name { get; }

        IField[] Fields { get; }

        IAnalysisSettings AnalysisSettings { get; }
    }
}
