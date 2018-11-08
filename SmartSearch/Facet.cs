using SmartSearch.Abstractions;
using System.Diagnostics;

namespace SmartSearch
{
    [DebuggerDisplay("{Name}: {Label} ({Count})")]
    public class Facet : IFacet
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public int Count { get; set; }

        public Facet()
        {
        }

        public Facet(string name, string label, int count)
        {
            Name = name;
            Label = label;
            Count = count;
        }
    }
}
