namespace SmartSearch.Abstractions
{
    public interface ISearchDomain
    {
        IAnalysisSettings AnalysisSettings { get; }
        IField[] Fields { get; }
        string Name { get; }
    }
}