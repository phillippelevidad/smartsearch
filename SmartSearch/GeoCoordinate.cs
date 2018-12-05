using System.Globalization;
using System.Text.RegularExpressions;
using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class GeoCoordinate : IGeoCoordinate
    {
        readonly Regex rgxPointWkt = new Regex(@"POINT\((\-?\d+(?:\.\d+))\s(\-?\d+(?:\.\d+))\)", RegexOptions.Compiled);

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public GeoCoordinate(double latitude, double longitude)
        {
            Initialize(latitude, longitude);
        }

        public GeoCoordinate(string wellKnownText)
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

        public string ToWellKnownText()
        {
            return string.Format(CultureInfo.InvariantCulture, "POINT({0:N8} {1:N8})", Longitude, Latitude);
        }

        public bool Equals(IGeoCoordinate other)
        {
            if (other == null)
                return false;

            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        void Initialize(double lat, double lng)
        {
            ValidateLatitude(lat);
            ValidateLongitude(lng);

            Latitude = lat;
            Longitude = lng;
        }

        void ValidateLatitude(double value)
        {
            if (value < -90 || value > 90)
                throw new InvalidLatitudeException(value);
        }

        void ValidateLongitude(double value)
        {
            if (value < -180 || value > 180)
                throw new InvalidLongitudeException(value);
        }
    }
}
