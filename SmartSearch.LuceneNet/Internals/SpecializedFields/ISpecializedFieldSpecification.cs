using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    interface ISpecializedFieldSpecification
    {
        ISpecializedField BuildFrom(IField field);

        object ConvertValue(ISpecializedField field, object value);

        bool IsEligibleToBecomeSpecialized(IField field);

        bool IsSatisfiedBy(ISpecializedField field);
    }
}
