using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class AnalyzedFieldSpecification : ISpecializedFieldSpecification
    {
        const string Suffix = "_analyzed";

        public ISpecializedField BuildFrom(IField field) =>
            new SpecializedField($"{field.Name}{Suffix}", field.Name, field.Type, field.Relevance,
                field.EnableFaceting, field.EnableSearching, field.EnableSorting);

        public object ConvertValue(ISpecializedField field, object value) =>
            value;

        public bool IsEligibleToBecomeSpecialized(IField field) =>
            field.EnableSearching && field.IsString();

        public bool IsSatisfiedBy(ISpecializedField field) =>
            field.Name.EndsWith(Suffix);
    }
}
