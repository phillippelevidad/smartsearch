using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class LatLngSortingShould
    {
        const string LocationField = "Geolocation";
        const string LocationNameField = "GeolocationName";

        readonly TestEnvironment environment;
        public LatLngSortingShould() => environment = TestEnvironment.Build();

        [TestMethod]
        public void Work()
        {
            TestInternal(new LatLngSortOptionReference(-25.427177, -49.256169),
                "Impact Hub Curitiba", "Parque Barigui", "Ouro Branco MG");

            TestInternal(new LatLngSortOptionReference(-20.669430, -43.785820),
                "Ouro Branco MG", "Impact Hub Curitiba", "Parque Barigui");
        }

        void TestInternal(ILatLngSortOptionReference reference, params string[] locationNamesInOrder)
        {
            var brazilBoxFilter = new Filter(LocationField, LatLngBoxFilterValue.Create(
                new LatLng(2.232406, -72.908444),
                new LatLng(-32.486597, -34.399626)));

            var results = environment.Search(new SearchRequest
            {
                Filters = new[] { brazilBoxFilter },
                SortOptions = new[] { new SortOption(LocationField, SortDirection.Ascending, reference) }
            });

            Assert.AreEqual(locationNamesInOrder.Length, results.TotalCount);

            var expectedNames = string.Join(",", locationNamesInOrder);
            var resultNames = string.Join(",", results.Documents.Select(d => d.Fields[LocationNameField].ToString()));

            Assert.AreEqual(expectedNames, resultNames);
        }
    }
}
