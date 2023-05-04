using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IDocumentReader reader;

        public DocumentProvider() : this(Array.Empty<IDocumentOperation>())
        {
        }

        public DocumentProvider(IEnumerable<IDocumentOperation> documents)
        {
            reader = new DocumentReader(documents);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public IDocumentReader GetDocumentReader() => reader;
    }

    public class DocumentReader : IDocumentReader
    {
        private int currentIndex = -1;

        public DocumentReader(IEnumerable<IDocumentOperation> documents)
        {
            Documents = documents.ToArray();
        }

        public IDocumentOperation CurrentDocument { get; private set; }
        public IDocumentOperation[] Documents { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
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
    }
}