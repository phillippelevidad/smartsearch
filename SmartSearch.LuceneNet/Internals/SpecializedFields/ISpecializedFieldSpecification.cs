﻿using SmartSearch.Abstractions;
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
        private readonly InternalSearchDomain searchDomain;

        public SpecializedFieldSpecifications(InternalSearchDomain searchDomain)
        {
            this.searchDomain = searchDomain;
        }

        public IEnumerable<ISpecializedFieldSpecification> ListAll()
        {
            yield return new ActionableLatLngFieldSpecification();
            yield return new AnalyzedFieldSpecification();
            yield return new SortableTextFieldSpecification();

            if (searchDomain.AnalysisSettings != null && searchDomain.AnalysisSettings.Synonyms.Any())
                yield return new SynonymFieldSpecification();
        }
    }
}