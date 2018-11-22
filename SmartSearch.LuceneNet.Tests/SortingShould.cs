using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public class SortingShould
    {
        [TestMethod]
        public void WorkForDateFields() => TestInternal("AddedDate");

        [TestMethod]
        public void WorkForLiteralFields() => TestInternal("Id");

        [TestMethod]
        public void WorkForNumericFields() => TestInternal("Price");

        [TestMethod]
        public void WorkForTextFields() => TestInternal("Name");

        void TestInternal(string fieldName)
        {
            TestInternal(fieldName, SortDirection.Ascending);
            TestInternal(fieldName, SortDirection.Descending);
        }

        void TestInternal(string fieldName, SortDirection direction)
        {
            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                SortOptions = new[] { new SortOption(fieldName, direction) }
            });

            var documentsAreSorted = AreDocumentsSorted(results.Documents, fieldName, direction == SortDirection.Descending);
            Assert.AreEqual(true, documentsAreSorted);
        }

        bool AreDocumentsSorted(IDocument[] documents, string fieldName, bool descending)
        {
            IComparable prev = null, current = null;
            var allSorted = true;

            foreach (var item in documents)
            {
                prev = current;
                current = (IComparable)item.Fields[fieldName];

                if (prev == null)
                    continue;

                var isSorted = descending
                    ? current.CompareTo(prev) <= 0
                    : current.CompareTo(prev) >= 0;

                if (!isSorted)
                {
                    allSorted = false;
                    break;
                }
            }

            return allSorted;
        }
    }
}
