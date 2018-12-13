using SmartSearch.Abstractions;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartSearch
{
    [DebuggerDisplay("Lat {Latitude}, Lng {Longitude}")]
    public class LatLng : ILatLng
    {
        private readonly Regex rgxPointWkt = new Regex(@"POINT\((\-?\d+(?:\.\d+))\s(\-?\d+(?:\.\d+))\)", RegexOptions.Compiled);

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public LatLng(double latitude, double longitude)
        {
            Initialize(latitude, longitude);
        }

        public LatLng(string wellKnownText)
        {
            if (!rgxPointWkt.IsMatch(wellKnownText))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            var m = rgxPointWkt.Match(wellKnownText);

            if (!double.TryParse(m.Groups[1].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double lng))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            if (!double.TryParse(m.Groups[2].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat))
                throw new MalformedPointWellKnownTextException(wellKnownText);

            Initialize(lat, lng);
        }

        public bool Equals(ILatLng other)
        {
            if (other == null)
                return false;

            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        // https://stackoverflow.com/a/5221407/484108
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Latitude.GetHashCode();
                hash = hash * 23 + Longitude.GetHashCode();
                return hash;
            }
        }

        public string ToWellKnownText()
        {
            return string.Format(CultureInfo.InvariantCulture, "POINT({0:N8} {1:N8})", Longitude, Latitude);
        }

        private void Initialize(double lat, double lng)
        {
            ValidateLatitude(lat);
            ValidateLongitude(lng);

            Latitude = lat;
            Longitude = lng;
        }

        private void ValidateLatitude(double value)
        {
            if (value < -90 || value > 90)
                throw new InvalidLatitudeException(value);
        }

        private void ValidateLongitude(double value)
        {
            if (value < -180 || value > 180)
                throw new InvalidLongitudeException(value);
        }
    }
}