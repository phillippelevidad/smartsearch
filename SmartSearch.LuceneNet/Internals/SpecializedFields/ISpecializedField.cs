using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal interface ISpecializedField : IField
    {
        string OriginalName { get; }
    }

    [DebuggerDisplay("{Name}, Type: {Type}, Relevance: {Relevance}")]
    internal class SpecializedField : ISpecializedField
    {
        public string Name { get; set; }

        public string OriginalName { get; set; }

        public FieldRelevance Relevance { get; set; }

        public FieldType Type { get; set; }

        public bool EnableFaceting { get; set; }

        public bool EnableSearching { get; set; }

        public bool EnableSorting { get; set; }

        public SpecializedField(string name, string originalName, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
        {
            Name = name;
            OriginalName = originalName;
            Type = type;
            Relevance = relevance;
            EnableFaceting = enableFaceting;
            EnableSearching = enableSearching;
            EnableSorting = enableSorting;
        }
    }
}
