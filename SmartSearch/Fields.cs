using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class Field : IField
    {
        public string Name { get; set; }

        public FieldType Type { get; set; }

        public bool EnableFaceting { get; set; }

        public bool EnableSearching { get; set; }

        public bool EnableReturning { get; set; }

        public bool EnableSorting { get; set; }

        public Field()
        {
        }

        public Field(string name, FieldType type)
        {
            Name = name;
            Type = type;
        }

        public Field(string name, FieldType type, bool enableFaceting = false, bool enableSearching = false, bool enableReturning = false, bool enableSorting = false)
        {
            Name = name;
            Type = type;
            EnableFaceting = enableFaceting;
            EnableSearching = enableSearching;
            EnableReturning = enableReturning;
            EnableSorting = enableSorting;
        }
    }
}
