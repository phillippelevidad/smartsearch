using SmartSearch.Abstractions;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    class MockDocumentProvider : IDocumentProvider
    {
        private readonly IDocumentReader reader;

        public MockDocumentProvider() : this(new IDocument[0])
        {
        }

        public MockDocumentProvider(IEnumerable<IDocument> documents)
        {
            reader = new MockDocumentReader(documents);
        }

        public IDocumentReader GetDocumentReader() => reader;

        public void Dispose() { }
    }
}
