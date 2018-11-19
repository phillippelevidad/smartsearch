﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class BoolFieldFilterShould
    {
        [TestMethod]
        public void WorkOnBooleanFieldWithTrueValue()
        {
            var fieldName = "IsInPromotion";

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                Filters = new[] { new Filter(fieldName, true) }
            });

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (bool)d.Fields[fieldName] == true);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }

        [TestMethod]
        public void WorkOnBooleanFieldWithFalseValue()
        {
            var fieldName = "IsInPromotion";

            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                Filters = new[] { new Filter(fieldName, false) }
            });

            var expectedCount = env.Documents.Count(d =>
                d.Fields.ContainsKey(fieldName) && (bool)d.Fields[fieldName] == false);

            Assert.AreEqual(expectedCount, results.TotalCount);
        }
    }
}
