using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    class MockDocumentReader : IDocumentReader
    {
        public IDocument CurrentDocument { get; private set; }

        public IDocument[] Documents { get; }

        int currentIndex = -1;

        public MockDocumentReader(IEnumerable<IDocument> documents)
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
