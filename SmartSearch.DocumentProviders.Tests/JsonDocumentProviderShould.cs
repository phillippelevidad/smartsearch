using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.DocumentProviders.Json;
using System.Collections.Generic;
using System.IO;

namespace SmartSearch.DocumentProviders.Tests
{
    [TestClass]
    public class JsonDocumentProviderShould
    {
        [TestMethod]
        public void ParseJsonDocument()
        {
            using (var stream = new MemoryStream(Resources.JsonData))
            using (var provider = new JsonDocumentProvider(stream, new JsonParser()))
            using (var reader = provider.GetDocumentReader())
            {
                List<IDocument> docs = new List<IDocument>();

                while (reader.ReadNext())
                    docs.Add(reader.CurrentDocument);

                Assert.AreEqual(docs.Count, 3);
            }
        }

        class JsonParser : IJsonDocumentParser
        {
            public IDocument Parse(dynamic jsonDocument)
            {
                return new Document(jsonDocument.Id.Value, new Dictionary<string, object>
                {
                    { "Title", (string)jsonDocument.Title.Value },
                    { "Description", (string)jsonDocument.Description.Value },
                    { "Price", (double)jsonDocument.Price },
                    { "PromotionalPrice", (double?)jsonDocument.PromotionalPrice?.Value },
                    { "IsInPromotion", (bool)jsonDocument.IsInPromotion.Value },
                    { "Category", (string)jsonDocument.Category.Value }
                });
            }
        }
    }
}
