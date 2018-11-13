using System;

namespace SmartSearch.Abstractions
{
    public interface IIndexContext : IDisposable
    {
        object GetContext();
    }
}
