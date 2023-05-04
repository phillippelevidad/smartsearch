using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName}, {Direction}, Reference = {Reference}")]
    public class SortOption : ISortOption
    {
        public SortDirection Direction { get; }
        public string FieldName { get; }
        public object Reference { get; }

        public SortOption(string fieldName, SortDirection direction)
        {
            FieldName = fieldName;
            Direction = direction;
        }

        public SortOption(string fieldName, SortDirection direction, object reference)
        {
            FieldName = fieldName;
            Direction = direction;
            Reference = reference;
        }
    }
}