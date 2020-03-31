using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public partial class FilterTests
    {
        [TestMethod]
        public void BoolFilterWorksWhenValueIsFalse()
        {
            var fieldName = "IsInPromotion";

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequestBuilder()
                .FilterBy(fieldName, false)
                .Build());

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (bool)d.Fields[fieldName] == false);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }

        [TestMethod]
        public void BoolFilterWorksWhenValueIsTrue()
        {
            var fieldName = "IsInPromotion";

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequestBuilder()
                .FilterBy(fieldName, true)
                .Build());

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (bool)d.Fields[fieldName] == true);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }

        [TestMethod]
        public void TextAndLiteralFiltersWork()
        {
            var env = TestEnvironment.Build(createIndex: false);
            env.SearchDomain = new SearchDomain("websites", new IField[]
            {
                new Field("Name", FieldType.Text, FieldRelevance.Normal, enableSearching: true),
                new Field("Categories", FieldType.TextArray),
                new Field("Url", FieldType.Literal)
            });

            // TODO: remover
            env.IndexContext = new PhysicalIndexContext(@"C:\Temp\SmartSearchIndexes\websites", false);

            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new DocumentProvider(new IDocumentOperation[]
            {
                DocumentOperation.AddOrUpdate("1", new Dictionary<string, object>
                {
                    { "Name", "Google" },
                    { "Categories", new[] { "Web Search" } },
                    { "Url", "https://www.google.com/" }
                }),
                DocumentOperation.AddOrUpdate("2", new Dictionary<string, object>
                {
                    { "Name", "Facebook" },
                    { "Categories", new[] { "Social Network" } },
                    { "Url", "https://www.facebook.com/" }
                }),
                DocumentOperation.AddOrUpdate("3", new Dictionary<string, object>
                {
                    { "Name", "Facebook Messenger" },
                    { "Categories", new[] { "Chat", "Social Network" } },
                    { "Url", "https://www.facebook.com/messenger/" }
                }),
                DocumentOperation.AddOrUpdate("4", new Dictionary<string, object>
                {
                    { "Name", "Instagram" },
                    { "Categories", new[] { "Social Network", "Mobile App" } },
                    { "Url", "https://www.instagram.com/" }
                })
            }));

            ISearchResult results;

            results = env.Search(new SearchRequest("facebook"));
            Assert.AreEqual(2, results.TotalCount);

            results = env.Search(new SearchRequestBuilder().FilterBy("Categories", "Social Network").Build());
            Assert.AreEqual(3, results.TotalCount);

            results = env.Search(new SearchRequestBuilder().FilterBy("Categories", "Mobile App").Build());
            Assert.AreEqual(1, results.TotalCount);

            results = env.Search(new SearchRequestBuilder().FilterBy("Url", "www.google.com").Build());
            Assert.AreEqual(0, results.TotalCount);

            results = env.Search(new SearchRequestBuilder().FilterBy("Url", "https://www.google.com/").Build());
            Assert.AreEqual(1, results.TotalCount);
        }
    }
}