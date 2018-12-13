using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    internal class MockSearchDomain : ISearchDomain
    {
        public IAnalysisSettings AnalysisSettings => new AnalysisSettings
        {
            Stopwords = new string[0],
            Synonyms = new string[0][]
        };

        public IField[] Fields => new[]
        {
            new Field("Id", FieldType.Literal, FieldRelevance.Normal, enableSorting: true),
            new Field("Name", FieldType.Text, FieldRelevance.Higher, enableSorting: true),
            new Field("Description", FieldType.Text, FieldRelevance.Normal, enableSearching: true),
            new Field("Price", FieldType.Double, FieldRelevance.Normal, enableSorting: true),
            new Field("PromotionalPrice", FieldType.Double, FieldRelevance.Normal, enableSorting: true),
            new Field("IsInPromotion", FieldType.Bool, FieldRelevance.Normal, enableSearching: true),
            new Field("Category", FieldType.Text, FieldRelevance.High, enableFaceting: true, enableSearching: true),
            new Field("AddedDate", FieldType.Date, FieldRelevance.Normal, enableSorting: true),
            new Field("GeolocationName", FieldType.Literal, FieldRelevance.Normal),
            new Field("Geolocation", FieldType.LatLng, FieldRelevance.Normal, enableSearching: true, enableSorting: true)
        };

        public string Name => "MockSearchDomain";
    }
}