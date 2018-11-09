using SmartSearch.LuceneNet.Internals;
using System.Linq;

namespace SmartSearch.Abstractions
{
    static class InternalSearchDomainExtensions
    {
        public static IField[] GetSearchEnabledFields(this InternalSearchDomain domain) =>
            domain.AllFields.Where(f => f.EnableSearching).ToArray();
    }
}
