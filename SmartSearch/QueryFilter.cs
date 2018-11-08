using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName} = {Value}")]
    public class QueryFilter : IQueryFilter
    {
        public string FieldName { get; set; }

        public object Value { get; set; }

        public QueryFilter()
        {
        }

        public QueryFilter(string fieldName, object value)
        {
            FieldName = fieldName;
            Value = value;
        }
    }
}
