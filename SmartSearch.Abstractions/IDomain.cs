namespace SmartSearch.Abstractions
{
    public interface IDomain
    {
        string Name { get; }

        IField[] Fields { get; }

        IAnalysisSettings AnalysisSettings { get; }
    }
}
