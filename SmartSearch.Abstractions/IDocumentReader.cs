using System;

namespace SmartSearch.Abstractions
{
    public interface IDocumentReader : IDisposable
    {
        IDocumentOperation CurrentDocument { get; }

        bool ReadNext();
    }
}