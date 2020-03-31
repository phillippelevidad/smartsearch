using System.Collections.ObjectModel;

namespace SmartSearch.Abstractions
{
    public interface IFilter
    {
    }

    public interface IFieldFilter : IFilter
    {
        string FieldName { get; }
    }

    public interface IValueFilter : IFieldFilter
    {
        object Value { get; }
    }

    public interface IRangeFilter : IFieldFilter
    {
        object FromValue { get; }
        object ToValue { get; }
    }

    public interface IFilterGroup : IFilter
    {
        ReadOnlyCollection<IFilter> Filters { get; }
        GroupingClause GroupingClause { get; }
    }
}
