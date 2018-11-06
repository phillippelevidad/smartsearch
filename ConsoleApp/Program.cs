using SmartSearch;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateIndex();
        }

        static void CreateIndex()
        {
            var domain = new Domain("testindex", new[]
            {
                new Field("Id", FieldType.Literal, enableSearching: true, enableReturning: true),
                new Field("Name", FieldType.Text, enableSearching: true, enableReturning: true),
                new Field("Color", FieldType.Literal, enableFaceting: true, enableSearching: true, enableReturning: true),
                new Field("Sizes", FieldType.LiteralArray, enableFaceting: true, enableSearching: true, enableReturning: true)
            });

            var indexService = new LuceneIndexService(new LuceneIndexOptions
            {
                IndexDirectory = @"C:\Temp\SmartSearchIndexes\"
            });

            using (var provider = new MockDocumentProvider())
            {
                indexService.CreateIndex(domain, provider);
            }
        }
    }

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
            new Document(Guid.NewGuid().ToString(), new Dictionary<string, object>
            {
                { "Id", "078be72c-1b47-4e9b-8939-a9485b8f5d5a" },
                { "Name", "Nike Shox" },
                { "Color", "Black" },
                { "Sizes", new [] { "35", "36", "37" } }
            }),
            new Document(Guid.NewGuid().ToString(), new Dictionary<string, object>
            {
                { "Id", "45ff8683-8498-4bbb-ab41-f5ce6a3c024e" },
                { "Name", "Nike Power Threading" },
                { "Color", "Red" },
                { "Sizes", new [] { "35", "36", "37", "38" } }
            }),
            new Document(Guid.NewGuid().ToString(), new Dictionary<string, object>
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
