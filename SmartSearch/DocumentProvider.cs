using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IDocumentReader reader;

        public DocumentProvider() : this(new IDocumentOperation[0])
        {
        }

        public DocumentProvider(IEnumerable<IDocumentOperation> documents)
        {
            reader = new DocumentReader(documents);
        }

        public IDocumentReader GetDocumentReader() => reader;

        public void Dispose() { }
    }

    public class DocumentReader : IDocumentReader
    {
        int currentIndex = -1;

        public IDocumentOperation CurrentDocument { get; private set; }

        public IDocumentOperation[] Documents { get; }

        public DocumentReader(IEnumerable<IDocumentOperation> documents)
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
