﻿using SmartSearch.Abstractions;

namespace ConsoleApp
{
    class MockDocumentProvider : IDocumentProvider
    {
        public void Dispose() { }

        public IDocumentReader GetDocumentReader() => new MockDocumentReader();
    }

    class MockDocumentReader : IDocumentReader
    {
        int index = -1;

        readonly IDocument[] documents = new[]
        {
            new Document("a53be0d5-f7fe-4c01-9845-9fffa5cd24f9", new Dictionary<string, object>
            {
                { "Id", "078be72c-1b47-4e9b-8939-a9485b8f5d5a" },
                { "Name", "Nike Shox" },
                { "Color", "Black" },
                { "Sizes", new [] { "35", "36", "37" } }
            }),
            new Document("0939df13-8748-4ffd-9306-f6ea54a6e92d", new Dictionary<string, object>
            {
                { "Id", "45ff8683-8498-4bbb-ab41-f5ce6a3c024e" },
                { "Name", "Nike Power Threading" },
                { "Color", "Red" },
                { "Sizes", new [] { "35", "36", "37", "38" } }
            }),
            new Document("ce33b619-d5be-4a77-8e5d-8809a44dce25", new Dictionary<string, object>
            {
                { "Id", "3aac080d-149a-4ed0-987f-c3487787d4cf" },
                { "Name", "Nike Sport" },
                { "Color", "Green" },
                { "Sizes", new [] { "35", "36", "37", "40", "42" } }
            })
        };

        public IDocument CurrentDocument => documents[index];

        public bool ReadNext() => ++index < documents.Length;

        public void Dispose() { }
    }
}
