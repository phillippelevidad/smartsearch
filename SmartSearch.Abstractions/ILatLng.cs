using System;

namespace SmartSearch.Abstractions
{
    public interface ILatLng : IEquatable<ILatLng>
    {
        double Latitude { get; }
        double Longitude { get; }
        string ToWellKnownText();
    }
}