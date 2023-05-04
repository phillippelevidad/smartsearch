using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Queries;
using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using LuceneFilter = Lucene.Net.Search.Filter;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    internal class FilterBuilder
    {
        private readonly InternalSearchDomain domain;

        public FilterBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public LuceneFilter Build(ISearchRequest request, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (request.Filters == null || request.Filters.Count == 0)
                return null;

            var wrapper = new FilterGroup(GroupingClause.And, request.Filters);
            return BuildByType(request, wrapper, perFieldAnalyzer);
        }

        private LuceneFilter BuildByType(ISearchRequest request, IFilter filter, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (filter is IFilterGroup filterGroup)
                return BuildFilterGroup(request, filterGroup, perFieldAnalyzer);

            if (filter is IFieldFilter fieldFilter)
                return BuildFieldFilter(request, fieldFilter, perFieldAnalyzer);

            throw new NotImplementedException($"FilterBuilder.BuildByType: unknown filter type {filter.GetType()} in SmartSearch.LuceneNet.Internals.Builders.");
        }

        private LuceneFilter BuildFilterGroup(ISearchRequest request, IFilterGroup group, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var booleanFilter = new BooleanFilter();
            foreach (var requestFilter in group.Filters)
            {
                var filter = BuildByType(request, requestFilter, perFieldAnalyzer);
                if (filter != null)
                    booleanFilter.Add(filter, group.GroupingClause == GroupingClause.And ? Occur.MUST : Occur.SHOULD);
            }

            return booleanFilter;
        }

        private LuceneFilter BuildFieldFilter(ISearchRequest request, IFieldFilter filter, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var field = domain.Fields.Single(f => f.Name == filter.FieldName);
            switch (field.Type)
            {
                case FieldType.Bool:
                case FieldType.BoolArray:
                    return new BoolFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.Date:
                case FieldType.DateArray:
                    return new DateFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.Double:
                case FieldType.DoubleArray:
                    return new DoubleFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.Int:
                case FieldType.IntArray:
                    return new IntFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.LatLng:
                    return new LatLngFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.Literal:
                case FieldType.LiteralArray:
                    return new LiteralFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                case FieldType.Text:
                case FieldType.TextArray:
                    return new TextFilterBuilder().Build(request, filter, field, perFieldAnalyzer);

                default:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}