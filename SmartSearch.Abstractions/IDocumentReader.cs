using System;

namespace SmartSearch.Abstractions
{
    public interface IDocumentReader : IDisposable
    {
        IDocument CurrentDocument { get; }

        bool ReadNext();
    }
}
