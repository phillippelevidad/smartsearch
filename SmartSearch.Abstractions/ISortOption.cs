namespace SmartSearch.Abstractions
{
    public interface ISortOption
    {
        string FieldName { get; }

        object Reference { get; }

        SortDirection Direction { get; }
    }

    public enum SortDirection
    {
        Ascending, Descending
    }
}
