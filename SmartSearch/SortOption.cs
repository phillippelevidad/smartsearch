using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{FieldName}, {Direction}")]
    public class SortOption : ISortOption
    {
        public string FieldName { get; set; }

        public SortDirection Direction { get; set; }

        public SortOption()
        {
        }

        public SortOption(string fieldName, SortDirection direction)
        {
            FieldName = fieldName;
            Direction = direction;
        }
    }
}
