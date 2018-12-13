using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.Converters;
using LuceneDocument = Lucene.Net.Documents.Document;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    internal class IndexDocumentBuilder
    {
        private readonly IDocumentConverter defaultDocumentConverter;
        private readonly InternalSearchDomain domain;
        private readonly IDocumentConverter facetDocumentConverter;
        private readonly FacetsConfig facetsConfig;
        private readonly ITaxonomyWriter taxonomyWriter;

        public IndexDocumentBuilder(InternalSearchDomain domain, FacetsConfig facetsConfig, ITaxonomyWriter taxonomyWriter)
        {
            this.domain = domain;
            this.facetsConfig = facetsConfig;
            this.taxonomyWriter = taxonomyWriter;
            defaultDocumentConverter = new DefaultDocumentConverter();
            facetDocumentConverter = new FacetDocumentConverter();
        }

        public LuceneDocument Build(IDocument document)
        {
            var internalDocument = InternalDocument.CreateFrom(domain, document);
            var mainDocument = defaultDocumentConverter.Convert(domain, internalDocument);
            var facetDocument = facetDocumentConverter.Convert(domain, internalDocument);

            foreach (var facet in facetDocument.Fields)
                mainDocument.Add(facet);

            return facetsConfig.Build(taxonomyWriter, mainDocument);
        }
    }
}