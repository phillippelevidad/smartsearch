namespace SmartSearch.Abstractions
{
    public interface ISortOption
    {
        string FieldName { get; }

        SortDirection Direction { get; }
    }

    public enum SortDirection
    {
        Ascending, Descending
    }
}
