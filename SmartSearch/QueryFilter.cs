using SmartSearch.Abstractions;

namespace SmartSearch
{
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
