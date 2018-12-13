using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface ILatLngFilterValue
    {
        IEnumerable<ILatLng> Points { get; }
    }
}