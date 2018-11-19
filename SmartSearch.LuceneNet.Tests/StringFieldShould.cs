using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class StringFieldShould
    {
        [TestMethod]
        public void HandleTextAndLiteralFiltersRight()
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

            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new MockDocumentProvider(new IDocumentOperation[]
            {
                new DocumentOperation("1", new Dictionary<string, object>
                {
                    { "Name", "Google" },
                    { "Categories", new[] { "Web Search" } },
                    { "Url", "https://www.google.com/" }
                }),
                new DocumentOperation("2", new Dictionary<string, object>
                {
                    { "Name", "Facebook" },
                    { "Categories", new[] { "Social Network" } },
                    { "Url", "https://www.facebook.com/" }
                }),
                new DocumentOperation("3", new Dictionary<string, object>
                {
                    { "Name", "Facebook Messenger" },
                    { "Categories", new[] { "Chat", "Social Network" } },
                    { "Url", "https://www.facebook.com/messenger/" }
                }),
                new DocumentOperation("4", new Dictionary<string, object>
                {
                    { "Name", "Instagram" },
                    { "Categories", new[] { "Social Network", "Mobile App" } },
                    { "Url", "https://www.instagram.com/" }
                })
            }));

            ISearchResult results;

            results = env.Search(new SearchRequest { Query = "facebook" });
            Assert.AreEqual(2, results.TotalCount);

            results = env.Search(new SearchRequest { Filters = new[] { new Filter("Categories", "Social Network") } });
            Assert.AreEqual(3, results.TotalCount);

            results = env.Search(new SearchRequest { Filters = new[] { new Filter("Categories", "Mobile App") } });
            Assert.AreEqual(1, results.TotalCount);

            results = env.Search(new SearchRequest { Filters = new[] { new Filter("Url", "www.google.com") } });
            Assert.AreEqual(0, results.TotalCount);

            results = env.Search(new SearchRequest { Filters = new[] { new Filter("Url", "https://www.google.com/") } });
            Assert.AreEqual(1, results.TotalCount);
        }
    }
}
