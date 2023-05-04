using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal interface ISpecializedFieldSpecification
    {
        ISpecializedField CreateFrom(IField field);

        bool IsEligibleForSpecialization(IField field);
    }
}