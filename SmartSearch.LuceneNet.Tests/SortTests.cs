using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System;

namespace SmartSearch.LuceneNet.Tests
{
    [TestClass]
    public partial class SortTests
    {
        [TestMethod]
        public void DateSortingWorks() => TestInternal("AddedDate");

        [TestMethod]
        public void DoubleSortingWorks() => TestInternal("Price");

        [TestMethod]
        public void IntSortingWorks() => TestInternal("Score");

        [TestMethod]
        public void LiteralSortingWorks() => TestInternal("Id");

        [TestMethod]
        public void TextSortingWorks() => TestInternal("Name");

        private void TestInternal(string fieldName)
        {
            TestInternal(fieldName, SortDirection.Ascending);
            TestInternal(fieldName, SortDirection.Descending);
        }

        private void TestInternal(string fieldName, SortDirection direction)
        {
            var env = TestEnvironment.Build();
            var results = env.Search(new SearchRequest
            {
                SortOptions = new[] { new SortOption(fieldName, direction) }
            });

            var documentsAreSorted = AreDocumentsSorted(results.Documents, fieldName, direction == SortDirection.Descending);
            Assert.AreEqual(true, documentsAreSorted);
        }

        private bool AreDocumentsSorted(IDocument[] documents, string fieldName, bool descending)
        {
            IComparable current = null;
            var allSorted = true;

            foreach (var item in documents)
            {
                IComparable prev = current;
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