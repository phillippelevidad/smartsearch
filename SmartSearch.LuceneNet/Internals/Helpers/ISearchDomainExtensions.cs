using System.Linq;

namespace SmartSearch.Abstractions
{
    static class ISearchDomainExtensions
    {
        public static IField[] GetFacetEnabledFields(this ISearchDomain domain) =>
            domain.Fields.Where(f => f.EnableFaceting).ToArray();

        public static IField[] GetSearchEnabledFields(this ISearchDomain domain) =>
            domain.Fields.Where(f => f.EnableSearching).ToArray();

        public static IField[] GetSortEnabledFields(this ISearchDomain domain) =>
            domain.Fields.Where(f => f.EnableSorting).ToArray();
    }
}
