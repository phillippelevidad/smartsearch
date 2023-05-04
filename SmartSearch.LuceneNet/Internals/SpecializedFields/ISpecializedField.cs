using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal interface ISpecializedField : IField
    {
        bool AnalyzeField { get; }
        string OriginalName { get; }
        float RelevanceBoostingMultiplier { get; }

        Type SpecialAnalyzerType { get; }

        object PrepareFieldValueForIndexing(object value);
    }
}