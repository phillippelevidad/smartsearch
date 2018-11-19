namespace SmartSearch.Abstractions
{
    public interface IFilter
    {
        string FieldName { get; }

        object SingleValue { get; }

        object RangeFrom { get; }

        object RangeTo { get; }

        FilterType FilterType { get; }
    }

    public enum FilterType
    {
        SingleValue, Range
    }
}
