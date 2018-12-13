namespace SmartSearch.Abstractions
{
    public enum FilterType
    {
        SingleValue, Range
    }

    public interface IFilter
    {
        string FieldName { get; }

        FilterType FilterType { get; }
        object RangeFrom { get; }
        object RangeTo { get; }
        object SingleValue { get; }
    }
}