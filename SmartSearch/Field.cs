using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class Field : IField
    {
        public string Name { get; set; }

        public FieldRelevance Relevance { get; set; }

        public FieldType Type { get; set; }

        public bool EnableFaceting { get; set; }

        public bool EnableSearching { get; set; }

        public bool EnableSorting { get; set; }

        public Field()
        {
        }

        public Field(string name, FieldType type) : this(name, type, FieldRelevance.Normal)
        {
        }

        public Field(string name, FieldType type, FieldRelevance relevance) : this(name, type, relevance, false, false, false)
        {
        }

        public Field(string name, FieldType type, FieldRelevance relevance, bool enableFaceting = false, bool enableSearching = false, bool enableSorting = false)
        {
            Name = name;
            Type = type;
            Relevance = relevance;
            EnableFaceting = enableFaceting;
            EnableSearching = enableSearching;
            EnableSorting = enableSorting;
        }
    }
}
