using SmartSearch.Abstractions;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartSearch
{
    [DebuggerDisplay("Lat {Latitude}, Lng {Longitude}")]
    public class LatLng : ILatLng
    {
        private static readonly Regex rgxPointWkt = new Regex(@"POINT\((\-?\d+(?:\.\d+))\s(\-?\d+(?:\.\d+))\)", RegexOptions.Compiled);

        public LatLng(double latitude, double longitude)
        {
            ThrowIfInvalidLat(latitude);
            ThrowIfInvalidLng(longitude);

            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public static LatLng FromWellKnownText(string wellKnownText)
        {
            if (!rgxPointWkt.IsMatch(wellKnownText))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            var m = rgxPointWkt.Match(wellKnownText);

            if (!double.TryParse(m.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double lng))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            if (!double.TryParse(m.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            return new LatLng(lat, lng);
        }

        public bool Equals(ILatLng other)
        {
            if (other == null)
                return false;
            return (Latitude, Longitude) == (other.Latitude, other.Longitude);
        }

        public string ToWellKnownText()
        {
            return string.Format(CultureInfo.InvariantCulture, "POINT({0:N8} {1:N8})", Longitude, Latitude);
        }

        private void ThrowIfInvalidLat(double value)
        {
            if (value < -90 || value > 90)
                throw new InvalidLatitudeException(value);
        }

        private void ThrowIfInvalidLng(double value)
        {
            if (value < -180 || value > 180)
                throw new InvalidLongitudeException(value);
        }
    }
}