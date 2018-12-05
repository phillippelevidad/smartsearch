using SmartSearch.Abstractions;
using System;

namespace SmartSearch
{
    public class LatLngSortOptionReference : ILatLngSortOptionReference
    {
        public ILatLng Origin { get; }

        public LatLngSortOptionReference(double latitude, double longitude)
            : this(new LatLng(latitude, longitude)) { }

        public LatLngSortOptionReference(ILatLng origin)
        {
            Origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }
    }
}
