using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch
{
    public class LatLngBoxFilterValue : LatLngFilterValueBase
    {
        public LatLngBoxFilterValue(ILatLng topLeft, ILatLng topRight, ILatLng bottomRight, ILatLng bottomLeft)
            : base(new[] { topLeft, topRight, bottomRight, bottomLeft, topLeft }) { }

        public static LatLngBoxFilterValue Create(ILatLng bottomLeft, ILatLng topRight)
        {
            if (bottomLeft == null)
                throw new ArgumentNullException(nameof(bottomLeft));

            if (topRight == null)
                throw new ArgumentNullException(nameof(topRight));

            double lat1 = bottomLeft.Latitude, lat2 = topRight.Latitude;
            double lng1 = bottomLeft.Longitude, lng2 = topRight.Longitude;

            var tl = new LatLng(
                Math.Max(lat1, lat2),
                Math.Min(lng1, lng2));

            var tr = new LatLng(
                Math.Max(lat1, lat2),
                Math.Max(lng1, lng2));

            var br = new LatLng(
                Math.Min(lat1, lat2),
                Math.Max(lng1, lng2));

            var bl = new LatLng(
                Math.Min(lat1, lat2),
                Math.Min(lng1, lng2));

            return new LatLngBoxFilterValue(tl, tr, br, bl);
        }
    }

    public abstract class LatLngFilterValueBase : ILatLngFilterValue
    {
        public IEnumerable<ILatLng> Points { get; protected set; }

        public LatLngFilterValueBase(IEnumerable<ILatLng> points)
        {
            if (points == null || !points.Any())
                throw new ArgumentNullException(nameof(points));

            if (!points.First().Equals(points.Last()))
                throw new FirstAndLastPointsInAPolygonMustBeEqualException(points);

            Points = points;
        }
    }

    // https://social.msdn.microsoft.com/Forums/sqlserver/en-US/46ec0b60-ec57-46bf-9873-9a16620b3f63/convert-circle-to-polygon?forum=sqlspatial
    public class LatLngRadiusFilterValue : LatLngFilterValueBase
    {
        private const double EarthRadiusInKm = 6371d;
        private const double EarthRadiusInMi = 3960d;
        private const double MaxRadiusInKm = 40008d;
        private const double MaxRadiusInMi = 24860d;

        private LatLngRadiusFilterValue(IEnumerable<ILatLng> points) : base(points)
        {
        }

        public static LatLngRadiusFilterValue CreateFromKmRadius(ILatLng origin, double radiusInKm)
        {
            return Create(origin, Math.Min(radiusInKm, MaxRadiusInKm) / EarthRadiusInKm);
        }

        public static LatLngRadiusFilterValue CreateFromMiRadius(ILatLng origin, double radiusInMi)
        {
            return Create(origin, Math.Min(radiusInMi, MaxRadiusInMi) / EarthRadiusInMi);
        }

        private static LatLngRadiusFilterValue Create(ILatLng origin, double d)
        {
            var lat = (origin.Latitude * Math.PI) / 180;
            var lon = (origin.Longitude * Math.PI) / 180;

            var polyPoints = new List<ILatLng>(362);

            for (var i = 0; i <= 360; i++)
            {
                var point = new LatLng(0, 0);
                var bearing = i * Math.PI / 180;

                var pointLatitude = (Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(bearing)) * 180) / Math.PI;
                var pointLongitude = (lon + Math.Atan2(Math.Sin(bearing) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(point.Latitude))) * 180 / Math.PI;

                polyPoints.Add(new LatLng(pointLatitude, pointLongitude));
            }

            if (!polyPoints.First().Equals(polyPoints.Last()))
            {
                var first = polyPoints.First();
                polyPoints[polyPoints.Count - 1] = new LatLng(first.Latitude, first.Longitude);
            }

            return new LatLngRadiusFilterValue(polyPoints);
        }
    }
}