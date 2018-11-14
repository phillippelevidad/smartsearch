using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class IndexServiceShould
    {
        [TestMethod]
        public void UpdateDocumentsWithExistingIds()
        {
            var env = TestEnvironment.Build(createIndex: false);
            env.SearchDomain = new SearchDomain("", new IField[]
            {
                new Field("Name", FieldType.Text),
                new Field("Age", FieldType.Int)
            });

            // Create a new index.
            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new MockDocumentProvider(new IDocument[]
            {
                new Document("1", new Dictionary<string, object>
                {
                    { "Name", "A" },
                    { "Age", 30 }
                })
            }));

            var fst_results = env.Search(new SearchRequest());
            var fst_a = fst_results.Documents.Single(d => d.Id == "1");

            Assert.AreEqual(fst_results.TotalCount, 1);
            Assert.AreEqual(fst_a.Fields["Name"], "A");
            Assert.AreEqual(fst_a.Fields["Age"], 30L);

            // Update a document and add another.
            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new MockDocumentProvider(new IDocument[]
            {
                new Document("1", new Dictionary<string, object> // should be updated
                {
                    { "Name", "A" },
                    { "Age", 35 }
                }),
                new Document("2", new Dictionary<string, object> // should be added
                {
                    { "Name", "B" },
                    { "Age", 40 }
                })
            }));

            var sec_results = env.Search(new SearchRequest());
            var sec_a = sec_results.Documents.Single(d => d.Id == "1");
            var sec_b = sec_results.Documents.Single(d => d.Id == "2");

            Assert.AreEqual(sec_results.TotalCount, 2);
            Assert.AreEqual(sec_a.Fields["Name"], "A");
            Assert.AreEqual(sec_a.Fields["Age"], 35L);
            Assert.AreEqual(sec_b.Fields["Name"], "B");
            Assert.AreEqual(sec_b.Fields["Age"], 40L);
        }
    }
}
