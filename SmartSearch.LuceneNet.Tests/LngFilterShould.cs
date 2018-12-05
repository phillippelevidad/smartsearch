using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class LngFilterShould
    {
        const string LocationField = "Geolocation";
        const string LocationNameField = "GeolocationName";

        readonly TestEnvironment environment;

        public LngFilterShould()
        {
            environment = TestEnvironment.Build();
        }

        [TestMethod]
        public void Work(/* :) */)
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

        void TestInternal(ILatLngFilterValue filterValue, params string[] locationNames)
        {
            var results = environment.Search(new SearchRequest
            {
                Filters = new[] { new Filter(LocationField, filterValue) }
            });

            Assert.AreEqual(locationNames.Length, results.TotalCount);

            var expectedNames = string.Join(",", locationNames);
            var resultNames = string.Join(",", results.Documents.Select(d => d.Fields[LocationNameField].ToString()));

            Assert.AreEqual(expectedNames, resultNames);
        }
    }
}
