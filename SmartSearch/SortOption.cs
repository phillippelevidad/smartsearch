using SmartSearch.Abstractions;

namespace SmartSearch
{
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
