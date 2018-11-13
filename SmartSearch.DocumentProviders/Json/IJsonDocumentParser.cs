using SmartSearch.Abstractions;

namespace SmartSearch.DocumentProviders.Json
{
    public interface IJsonDocumentParser
    {
        IDocument Parse(dynamic jsonDocument);
    }
}
