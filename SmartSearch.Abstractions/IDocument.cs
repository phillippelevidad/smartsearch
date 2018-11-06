using System.Collections.Generic;

namespace SmartSearch.Abstractions
{
    public interface IDocument
    {
        string Id { get; }

        IDictionary<string, object> Fields { get; }
    }
}
