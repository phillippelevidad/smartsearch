using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class RangeFilterShould
    {
        [TestMethod]
        public void WorkForNumericGreaterThan()
        {
            var fieldName = "Price";
            var fromValue = 1000d;

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                Filters = new[] { new Filter(fieldName, fromValue, null) }
            });

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (double)d.Fields[fieldName] >= fromValue);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }

        [TestMethod]
        public void WorkForNumericLowerThan()
        {
            var fieldName = "Price";
            var toValue = 1000d;

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                Filters = new[] { new Filter(fieldName, null, toValue) }
            });

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (double)d.Fields[fieldName] <= toValue);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }

        [TestMethod]
        public void WorkForNumericFullySpecifiedRange()
        {
            var fieldName = "Price";
            var fromValue = 100d;
            var toValue = 1000d;

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                Filters = new[] { new Filter(fieldName, fromValue, toValue) }
            });

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) &&
                    (double)d.Fields[fieldName] >= fromValue &&
                    (double)d.Fields[fieldName] <= toValue);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }
    }
}
