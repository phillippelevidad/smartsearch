using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    public partial class FilterTests
    {
        private const string LocationField = "Geolocation";
        private const string LocationNameField = "GeolocationName";

        [TestMethod]
        public void LatLngFilterWorks()
        {
            var env = TestEnvironment.Build();

            TestInternal(env, LatLngRadiusFilterValue.CreateFromKmRadius(new LatLng(-25.428595, -49.258507), 1),
                "Impact Hub Curitiba");

            TestInternal(env, LatLngRadiusFilterValue.CreateFromKmRadius(new LatLng(-25.429596, -49.271271), 20),
                "Impact Hub Curitiba", "Parque Barigui");

            TestInternal(env, LatLngBoxFilterValue.Create(new LatLng(-25.573163, -49.368852), new LatLng(-25.353702, -49.181480)),
                "Impact Hub Curitiba", "Parque Barigui");

            TestInternal(env, LatLngBoxFilterValue.Create(new LatLng(44.477891, 5.110339), new LatLng(50.692108, 15.345120)),
                "Geneva Switzerland", "Lausanne Switzerland");

            TestInternal(env, LatLngBoxFilterValue.Create(new LatLng(37.809352, -122.521424), new LatLng(37.608588, -122.279663)),
                "San Francisco US");

            TestInternal(env, LatLngBoxFilterValue.Create(new LatLng(34.134542, -16.374786), new LatLng(-32.722599, 50.392885))
                /* no results expected */);
        }

        private void TestInternal(TestEnvironment environment, ILatLngFilterValue filterValue, params string[] locationNames)
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