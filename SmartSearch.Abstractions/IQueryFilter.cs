namespace SmartSearch.Abstractions
{
    public interface IQueryFilter
    {
        string FieldName { get; }

        object Value { get; }
    }
}
