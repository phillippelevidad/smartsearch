using System;

namespace SmartSearch.Abstractions
{
    public interface IGeoCoordinate : IEquatable<IGeoCoordinate>
    {
        double Latitude { get; }

        double Longitude { get; }

        string ToWellKnownText();
    }
}
