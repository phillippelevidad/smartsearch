using Lucene.Net.Facet;
using Lucene.Net.Facet.Taxonomy;
using Lucene.Net.Facet.Taxonomy.Directory;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals;
using SmartSearch.LuceneNet.Internals.Converters;
using SmartSearch.LuceneNet.Internals.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet
{
    public class LuceneSearchService : ISearchService
    {
        readonly LuceneIndexOptions options;
        readonly IDocumentConverter documentConverter;

        public LuceneSearchService() : this(new LuceneIndexOptions())
        {
        }

        public LuceneSearchService(LuceneIndexOptions options)
        {
            this.options = options;
            documentConverter = new DefaultDocumentConverter();
        }

        public ISearchResult Search(ISearchDomain domain, ISearchRequest request)
        {
            using (CultureContext.Invariant)
            {
                var internalDomain = InternalSearchDomain.CreateFrom(domain);

                using (var facetsReader = GetFacetsReader(domain))
                using (var indexReader = GetIndexReader(domain))
                {
                    var factory = new InternalAnalyzerFactory(internalDomain, options.AnalyzerFactory);
                    var analyzer = factory.Create();

                    var searcher = new IndexSearcher(indexReader);
                    var query = new QueryBuilder(internalDomain).Build(request, analyzer);
                    var sort = new SortBuilder(internalDomain).Build(request);

                    return SearchInternal(internalDomain, request, searcher, facetsReader, query, sort);
                }
            }
        }

        TaxonomyReader GetFacetsReader(ISearchDomain domain)
        {
            var path = IndexDirectoryHelper.GetFacetsDirectoryPath(options.IndexDirectory, domain.Name);
            return new DirectoryTaxonomyReader(FSDirectory.Open(path));
        }

        DirectoryReader GetIndexReader(ISearchDomain domain)
        {
            var path = IndexDirectoryHelper.GetDirectoryPath(options.IndexDirectory, domain.Name);
            return DirectoryReader.Open(FSDirectory.Open(path));
        }

        ISearchResult SearchInternal(
            InternalSearchDomain domain, ISearchRequest request,
            IndexSearcher searcher, TaxonomyReader facetsReader,
            Query query, Sort sort)
        {
            var facetsCollector = new FacetsCollector();
            var results = FacetsCollector.Search(searcher, query, int.MaxValue, facetsCollector);

            var documents = CollectDocumentResults(domain, request, searcher, results);
            var facets = CollectFacetResults(domain, results, facetsCollector, facetsReader);

            return new SearchResult(documents, facets, results.TotalHits);
        }

        IDocument[] CollectDocumentResults(InternalSearchDomain domain, ISearchRequest request, IndexSearcher searcher, TopDocs searchResults)
        {
            var hits = searchResults.ScoreDocs;

            var start = request.StartIndex;
            var end = Math.Min(searchResults.TotalHits, request.StartIndex + request.PageSize);
            var resultSize = end - start;

            var items = new IDocument[resultSize];
            var itemIndex = 0;

            for (var i = start; i < end; i++)
            {
                var luceneDocument = searcher.Doc(hits[i].Doc);
                var documentResult = documentConverter.Convert(domain, luceneDocument);

                items[itemIndex++] = documentResult;
            }

            return items;
        }

        IFacet[] CollectFacetResults(ISearchDomain domain, TopDocs searchResults, FacetsCollector facetsCollector, TaxonomyReader facetsReader)
        {
            var facetFields = domain.Fields.Where(f => f.EnableFaceting);

            var results = new List<Facet>();
            var config = new FacetsConfig();
            var facets = new FastTaxonomyFacetCounts(facetsReader, config, facetsCollector);

            foreach (var f in facetFields)
            {
                var luceneFacet = facets.GetTopChildren(30, f.Name);

                if (luceneFacet == null)
                    continue;

                var facetResults = luceneFacet.LabelValues.Select(lv => new Facet(f.Name, lv.Label, (int)lv.Value));
                results.AddRange(facetResults);
            }

            return results.ToArray();
        }
    }
}
