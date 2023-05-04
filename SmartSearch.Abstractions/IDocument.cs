using System.Collections.ObjectModel;

namespace SmartSearch.Abstractions
{
    public interface IDocument
    {
        string Id { get; }
        ReadOnlyDictionary<string, object> Fields { get; }
    }
}