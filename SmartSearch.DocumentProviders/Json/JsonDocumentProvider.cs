using SmartSearch.Abstractions;
using System;
using System.IO;

namespace SmartSearch.DocumentProviders.Json
{
    public class JsonDocumentProvider : IDocumentProvider
    {
        bool disposed;

        public Stream Stream { get; protected set; }

        public IJsonDocumentParser Parser { get; protected set; }

        public JsonDocumentReader Reader { get; protected set; }

        public JsonDocumentProvider(string jsonFilePath, IJsonDocumentParser parser)
            : this(File.OpenRead(jsonFilePath), parser)
        {
        }

        public JsonDocumentProvider(Stream jsonStream, IJsonDocumentParser parser)
        {
            Stream = jsonStream ?? throw new ArgumentNullException(nameof(jsonStream));
            Parser = parser;
        }

        public IDocumentReader GetDocumentReader()
        {
            if (Reader == null)
                Reader = new JsonDocumentReader(Stream, Parser);

            return Reader;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (disposing && !disposed)
            {
                Stream?.Dispose();
                Reader?.Dispose();
                disposed = true;
            }
        }
    }
}
