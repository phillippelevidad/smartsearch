using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    public partial class LatLngFilterTests
    {
        private const string LocationField = "Geolocation";
        private const string LocationNameField = "GeolocationName";

        private readonly TestEnvironment environment;

        public LatLngFilterTests() => environment = TestEnvironment.Build();

        [TestMethod]
        public void LatLngFilterWorks(/* :) */)
        {
            TestInternal(LatLngBoxFilterValue.Create(new LatLng(-25.573163, -49.368852), new LatLng(-25.353702, -49.181480)),
                "Impact Hub Curitiba", "Parque Barigui");

            TestInternal(LatLngBoxFilterValue.Create(new LatLng(44.477891, 5.110339), new LatLng(50.692108, 15.345120)),
                "Geneva Switzerland", "Lausanne Switzerland");

            TestInternal(LatLngBoxFilterValue.Create(new LatLng(37.809352, -122.521424), new LatLng(37.608588, -122.279663)),
                "San Francisco US");

            TestInternal(LatLngBoxFilterValue.Create(new LatLng(34.134542, -16.374786), new LatLng(-32.722599, 50.392885))
                /* no results expected */);
        }

        private void TestInternal(ILatLngFilterValue filterValue, params string[] locationNames)
        {
            var results = environment.Search(new SearchRequest
            {
                Filters = new[] { new Filter(LocationField, filterValue) }
            });

            var resultNames = results.Documents.Select(d => d.Fields[LocationNameField].ToString()).ToArray();
            var intersectionCount = locationNames.Intersect(resultNames).Count();

            Assert.AreEqual(locationNames.Length, intersectionCount);
        }
    }
}