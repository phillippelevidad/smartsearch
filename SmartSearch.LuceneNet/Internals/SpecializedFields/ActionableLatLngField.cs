using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal class ActionableLatLngField : Field, ISpecializedField
    {
        private const string Suffix = "_latlng";

        public bool AnalyzeField => true;
        public string OriginalName { get; }

        public Type SpecialAnalyzerType => null;

        public ActionableLatLngField(string name, FieldType type, FieldRelevance relevance)
            : base(name + Suffix, type, relevance)
        {
            OriginalName = name;
        }

        public object PrepareFieldValueForIndexing(object value) =>
            value;
    }

    internal class ActionableLatLngFieldSpecification : ISpecializedFieldSpecification
    {
        public ISpecializedField CreateFrom(IField field) =>
            new ActionableLatLngField(field.Name, field.Type, field.Relevance);

        public bool IsEligibleForSpecialization(IField field) =>
                    field.Type == FieldType.LatLng;
    }
}