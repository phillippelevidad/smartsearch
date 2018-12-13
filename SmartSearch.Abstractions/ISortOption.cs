namespace SmartSearch.Abstractions
{
    public enum SortDirection
    {
        Ascending, Descending
    }

    public interface ISortOption
    {
        SortDirection Direction { get; }
        string FieldName { get; }

        object Reference { get; }
    }
}