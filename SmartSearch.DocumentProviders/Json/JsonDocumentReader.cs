using Newtonsoft.Json;
using SmartSearch.Abstractions;
using System;
using System.IO;
using System.Text;

namespace SmartSearch.DocumentProviders.Json
{
    public class JsonDocumentReader : IDocumentReader
    {
        bool disposed;
        dynamic[] jsonObjects;
        int currentIndex = -1;

        public IDocument CurrentDocument { get; protected set; }

        public Stream Stream { get; }

        public IJsonDocumentParser Parser { get; }

        public JsonDocumentReader(Stream jsonStream, IJsonDocumentParser parser)
        {
            Stream = jsonStream ?? throw new ArgumentNullException(nameof(jsonStream));
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public bool ReadNext()
        {
            if (jsonObjects == null)
                InitializeJsonObjects();

            if (++currentIndex < jsonObjects.Length)
            {
                CurrentDocument = Parser.Parse(jsonObjects[currentIndex]);
                return true;
            }
            else
            {
                CurrentDocument = null;
                return false;
            }
        }

        void InitializeJsonObjects()
        {
            var streamReader = new StreamReader(Stream, Encoding.UTF8);
            var jsonString = streamReader.ReadToEnd();

            jsonObjects = JsonConvert.DeserializeObject<dynamic[]>(jsonString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                Stream.Dispose();
                disposed = true;
            }
        }
    }
}
