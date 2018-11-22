using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    interface ISpecializedField : IField
    {
        string OriginalName { get; }

        Type AnalyzerType { get; }

        bool AnalyzeField { get; }

        object PrepareFieldValueForIndexing(object value);
    }
}
