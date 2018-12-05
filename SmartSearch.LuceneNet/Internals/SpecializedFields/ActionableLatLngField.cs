using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class ActionableLatLngField : Field, ISpecializedField
    {
        const string Suffix = "_latlng";

        public string OriginalName { get; }

        public Type SpecialAnalyzerType => null;

        public bool AnalyzeField => true;

        public ActionableLatLngField(string name, FieldType type, FieldRelevance relevance)
            : base(name + Suffix, type, relevance)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) =>
            value;
    }

    class ActionableLatLngFieldSpecification : ISpecializedFieldSpecification
    {
        public bool IsEligibleForSpecialization(IField field) =>
            field.Type == FieldType.LatLng;

        public ISpecializedField CreateFrom(IField field) =>
            new ActionableLatLngField(field.Name, field.Type, field.Relevance);
    }
}
