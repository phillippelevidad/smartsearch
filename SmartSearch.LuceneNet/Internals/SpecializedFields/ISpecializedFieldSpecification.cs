using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    interface ISpecializedFieldSpecification
    {
        bool IsEligibleForSpecialization(IField field);

        ISpecializedField CreateFrom(IField field);
    }

    static class SpecializedFieldSpecifications
    {
        public static ISpecializedFieldSpecification[] ListAll() => new ISpecializedFieldSpecification[]
        {
            new AnalyzedFieldSpecification(),
            new SynonymFieldSpecification()
        };
    }
}
