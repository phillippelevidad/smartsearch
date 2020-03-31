using SmartSearch.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    internal interface ISpecializedFieldSpecification
    {
        ISpecializedField CreateFrom(IField field);

        bool IsEligibleForSpecialization(IField field);
    }

    internal class SpecializedFieldSpecifications
    {
        private readonly IAnalysisSettings analysisSettings;

        public SpecializedFieldSpecifications(IAnalysisSettings analysisSettings)
        {
            this.analysisSettings = analysisSettings;
        }

        public IEnumerable<ISpecializedFieldSpecification> ListAll()
        {
            yield return new ActionableLatLngFieldSpecification();
            yield return new AnalyzedFieldSpecification();
            yield return new SortableTextFieldSpecification();

            if (analysisSettings != null && analysisSettings.Synonyms.Any())
                yield return new SynonymFieldSpecification();
        }
    }
}