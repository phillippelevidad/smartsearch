using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class IndexServiceTests
    {
        [TestMethod]
        public void ExistingDocumentsAreUpdated()
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

            Assert.AreEqual(1, fst_results.TotalCount);
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

            Assert.AreEqual(2, sec_results.TotalCount);
            Assert.AreEqual("A", sec_a.Fields["Name"]);
            Assert.AreEqual(35L, sec_a.Fields["Age"]);
            Assert.AreEqual("B", sec_b.Fields["Name"], "B");
            Assert.AreEqual(40L, sec_b.Fields["Age"]);
        }
    }
}