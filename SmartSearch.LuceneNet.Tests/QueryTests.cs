using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.LuceneNet.Tests.Mocks;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void EmptyQueryReturnsAllResults()
        {
            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest { Query = "" });
            Assert.AreEqual(env.Documents.Length, results.TotalCount);
        }

        [TestMethod]
        public void WhitespaceQueryReturnsAllResults()
        {
            var env = TestEnvironment.Build();
            var results = env.SearchService.Search(env.IndexContext, env.SearchDomain, new SearchRequest { Query = " " });
            Assert.AreEqual(env.Documents.Length, results.TotalCount);
        }
    }
}