using System;

namespace SmartSearch.Abstractions
{
    public interface IDocumentProvider : IDisposable
    {
        IDocumentReader GetDocumentReader();
    }
}
