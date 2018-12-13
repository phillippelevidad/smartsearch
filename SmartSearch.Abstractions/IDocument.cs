using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface IDocument
    {
        IDictionary<string, object> Fields { get; }
        string Id { get; }
    }
}