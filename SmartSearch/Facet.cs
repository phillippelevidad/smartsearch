using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Name}: {Label} ({Count})")]
    public class Facet : IFacet
    {
        public int Count { get; }
        public string Label { get; }
        public string Name { get; }

        public Facet(string name, string label, int count)
        {
            Name = name;
            Label = label;
            Count = count;
        }
    }
}