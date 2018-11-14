namespace SmartSearch.Abstractions
{
    public interface IQueryFilter
    {
        string FieldName { get; }

        object SingleValue { get; }

        object RangeFrom { get; }

        object RangeTo { get; }

        QueryFilterType FilterType { get; }
    }

    public enum QueryFilterType
    {
        SingleValue, Range
    }
}
