namespace SmartSearch.Abstractions
{
    public interface IFacet
    {
        int Count { get; }
        string Label { get; }
        string Name { get; }
    }
}