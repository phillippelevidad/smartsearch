using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.LuceneNet.Tests.Mocks;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class EmptyQueryShould
    {
        [TestMethod]
        public void ReturnAllResults()
        {
            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest { Query = "" });
            Assert.AreEqual(env.Documents.Length, results.TotalCount);
        }
    }
}