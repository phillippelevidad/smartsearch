using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Name}, Type: {Type}, Relevance: {Relevance}")]
    public class Field : IField
    {
        public bool EnableFaceting { get; }
        public bool EnableSearching { get; }
        public bool EnableSorting { get; }
        public string Name { get; }
        public FieldRelevance Relevance { get; }
        public float RelevanceBoost { get; }
        public FieldType Type { get; }

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
            RelevanceBoost = FieldRelevanceBoost.GetBoostValue(relevance);

            EnableFaceting = enableFaceting;
            EnableSearching = enableSearching;
            EnableSorting = enableSorting;
        }
    }
}