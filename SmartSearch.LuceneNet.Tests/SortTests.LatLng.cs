using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    public partial class SortTests
    {
        private const string LocationField = "Geolocation";
        private const string LocationNameField = "GeolocationName";

        [TestMethod]
        public void LatLngSortingWorks()
        {
            var environment = TestEnvironment.Build();

            TestInternal(environment, new LatLng(-25.427177, -49.256169),
                "Impact Hub Curitiba", "Parque Barigui", "Ouro Branco MG");

            TestInternal(environment, new LatLng(-20.669430, -43.785820),
                "Ouro Branco MG", "Impact Hub Curitiba", "Parque Barigui");
        }

        private void TestInternal(TestEnvironment environment, ILatLng reference, params string[] locationNamesInOrder)
        {
            var brazilBoxFilter = LatLngFilter.CreateBox(LocationField,
                new LatLng(2.232406, -72.908444),
                new LatLng(-32.486597, -34.399626));

            var results = environment.Search(new SearchRequestBuilder()
                .FilterBy(brazilBoxFilter)
                .SortBy(LocationField, SortDirection.Ascending, reference)
                .Build());

            Assert.AreEqual(locationNamesInOrder.Length, results.TotalCount);

            var expectedNames = string.Join(",", locationNamesInOrder);
            var resultNames = string.Join(",", results.Documents.Select(d => d.Fields[LocationNameField].ToString()));

            Assert.AreEqual(expectedNames, resultNames);
        }
    }
}