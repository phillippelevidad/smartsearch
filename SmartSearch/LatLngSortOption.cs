using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class LatLngSortOption : SortOption
    {
        public LatLngSortOption(string fieldName, SortDirection direction, double latitude, double longitude)
            : base(fieldName, direction, new LatLng(latitude, longitude)) { }

        public LatLngSortOption(string fieldName, SortDirection direction, ILatLng origin)
            : base(fieldName, direction, origin) { }
    }
}