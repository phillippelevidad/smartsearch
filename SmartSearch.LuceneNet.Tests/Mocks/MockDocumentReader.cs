using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    class MockDocumentReader : IDocumentReader
    {
        int currentIndex = -1;

        public IDocumentOperation CurrentDocument { get; private set; }

        public IDocumentOperation[] Documents { get; }

        public MockDocumentReader(IEnumerable<IDocumentOperation> documents)
        {
            Documents = documents.ToArray();
        }

        public bool ReadNext()
        {
            if (++currentIndex < Documents.Length)
            {
                CurrentDocument = Documents[currentIndex];
                return true;
            }
            else
            {
                CurrentDocument = null;
                return false;
            }
        }

        public void Dispose() { }
    }
}
