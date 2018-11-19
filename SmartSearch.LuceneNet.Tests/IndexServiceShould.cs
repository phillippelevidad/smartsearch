using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;
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
            env.SearchDomain = new SearchDomain("indexupdate", new IField[]
            {
                new Field("Name", FieldType.Text),
                new Field("Age", FieldType.Int)
            });

            var aId = Guid.NewGuid().ToString();
            var bId = Guid.NewGuid().ToString();

            // Create a new index.
            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new DocumentProvider(new IDocumentOperation[]
            {
                new DocumentOperation(aId, new Dictionary<string, object>
                {
                    { "Name", "A" },
                    { "Age", 30 }
                })
            }));

            var fst_results = env.Search(new SearchRequest());
            var fst_a = fst_results.Documents.Single(d => d.Id == aId);

            Assert.AreEqual(fst_results.TotalCount, 1);
            Assert.AreEqual(fst_a.Fields["Name"], "A");
            Assert.AreEqual(fst_a.Fields["Age"], 30L);

            // Update a document and add another.
            env.IndexService.CreateIndex(env.IndexContext, env.SearchDomain, new DocumentProvider(new IDocumentOperation[]
            {
                new DocumentOperation(aId, new Dictionary<string, object> // should be updated
                {
                    { "Name", "A" },
                    { "Age", 35 }
                }),
                new DocumentOperation(bId, new Dictionary<string, object> // should be added
                {
                    { "Name", "B" },
                    { "Age", 40 }
                })
            }));

            var sec_results = env.Search(new SearchRequest());
            var sec_a = sec_results.Documents.Single(d => d.Id == aId);
            var sec_b = sec_results.Documents.Single(d => d.Id == bId);

            Assert.AreEqual(sec_results.TotalCount, 2);
            Assert.AreEqual(sec_a.Fields["Name"], "A");
            Assert.AreEqual(sec_a.Fields["Age"], 35L);
            Assert.AreEqual(sec_b.Fields["Name"], "B");
            Assert.AreEqual(sec_b.Fields["Age"], 40L);
        }
    }
}
