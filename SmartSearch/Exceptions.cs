using System;
using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class MalformedPointWellKnownTextException : Exception
    {
        public MalformedPointWellKnownTextException(string wkt)
            : base($"Malformed Well Known-Text '{wkt}'. Must be in the format: 'POINT(<longitude> <latitude>)'.") { }
    }

    public class InvalidLatitudeException : Exception
    {
        public InvalidLatitudeException(double latitude)
            : base($"Invalid latitude value: {latitude}. Must be between -90 and 90 (inclusive).") { }
    }

    public class InvalidLongitudeException : Exception
    {
        public InvalidLongitudeException(double latitude)
            : base($"Invalid longitude value: {latitude}. Must be between -180 and 180 (inclusive).") { }
    }

    public class UnknownQueryFilterTypeException : Exception
    {
        public UnknownQueryFilterTypeException(FilterType filterType)
            : base($"Query filter type '{filterType}' is not recognized.") { }
    }
}
