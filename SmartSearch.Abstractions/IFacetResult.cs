namespace SmartSearch.Abstractions
{
    public interface IFacet
    {
        string Name { get; }

        string Label { get; }

        int Count { get; }
    }
}
