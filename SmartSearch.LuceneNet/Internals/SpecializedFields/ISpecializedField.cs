using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal interface ISpecializedField : IField
    {
        string OriginalName { get; }
    }

    [DebuggerDisplay("{Name}, Type: {Type}, Relevance: {Relevance}")]
    internal class SpecializedField : Field, ISpecializedField
    {
        public string OriginalName { get; set; }

        public SpecializedField(string name, string originalName, FieldType type, FieldRelevance relevance, bool enableFaceting, bool enableSearching, bool enableSorting)
            : base(name, type, relevance, enableFaceting, enableSearching, enableSorting)
        {
            OriginalName = originalName;
        }
    }
}
