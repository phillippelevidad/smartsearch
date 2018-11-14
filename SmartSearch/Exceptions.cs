using System;
using SmartSearch.Abstractions;

namespace SmartSearch
{
    public class UnknownQueryFilterTypeException : Exception
    {
        public UnknownQueryFilterTypeException(QueryFilterType filterType)
            : base($"Query filter type '{filterType}' is not recognized.") { }
    }
}
