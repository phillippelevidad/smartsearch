using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName} = {Value}")]
    public class ValueFilter : IValueFilter
    {
        public ValueFilter(string fieldName, object value)
        {
            FieldName = fieldName;
            Value = value;
        }

        public string FieldName { get; }
        public object Value { get; }
    }

    [DebuggerDisplay("{FieldName} = (from {FromValue} to {ToValue})")]
    public class RangeFilter : IRangeFilter
    {
        public RangeFilter(string fieldName, object fromValue, object toValue)
        {
            FieldName = fieldName;
            FromValue = fromValue;
            ToValue = toValue;
        }

        public string FieldName { get; }
        public object FromValue { get; }
        public object ToValue { get; }
    }

    [DebuggerDisplay("[{GroupingClause}] {Filters.Count} filter(s)")]
    public class FilterGroup : IFilterGroup
    {
        public FilterGroup(GroupingClause groupingClause, IEnumerable<IFilter> filters)
        {
            GroupingClause = groupingClause;
            Filters = (filters ?? Array.Empty<IFilter>()).ToList().AsReadOnly();
        }

        public FilterGroup(GroupingClause groupingClause, params IFilter[] filters)
            : this(groupingClause, (IEnumerable<IFilter>)filters) { }

        public GroupingClause GroupingClause { get; }
        public ReadOnlyCollection<IFilter> Filters { get; }
    }
}