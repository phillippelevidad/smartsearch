using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Queries;
using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class FilterBuilder
    {
        readonly InternalSearchDomain domain;

        public FilterBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public Filter Build(ISearchRequest request, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (request.Filters == null || request.Filters.Length == 0)
                return null;

            var booleanFilter = new BooleanFilter();

            foreach (var requestFilter in request.Filters)
            {
                var field = domain.Fields.Single(f => f.Name == requestFilter.FieldName);
                var filter = BuildFilterForField(request, requestFilter, field, perFieldAnalyzer);

                if (filter != null)
                    booleanFilter.Add(filter, Occur.MUST);
            }

            return booleanFilter;
        }

        Filter BuildFilterForField(ISearchRequest request, IQueryFilter requestFilter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            switch (field.Type)
            {
                case FieldType.Bool:
                case FieldType.BoolArray:
                    return new BoolFilterBuilder().Build(request, requestFilter, field, perFieldAnalyzer);

                case FieldType.Date:
                case FieldType.DateArray:
                    return new DateFilterBuilder().Build(request, requestFilter, field, perFieldAnalyzer);

                case FieldType.Double:
                case FieldType.DoubleArray:
                    return new DoubleFilterBuilder().Build(request, requestFilter, field, perFieldAnalyzer);

                case FieldType.Int:
                case FieldType.IntArray:
                    return new IntFilterBuilder().Build(request, requestFilter, field, perFieldAnalyzer);

                case FieldType.Literal:
                case FieldType.LiteralArray:
                case FieldType.Text:
                case FieldType.TextArray:
                    return new TextAndLiteralFilterBuilder().Build(request, requestFilter, field, perFieldAnalyzer);

                default:
                case FieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}
