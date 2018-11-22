using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class SortBuilder
    {
        readonly InternalSearchDomain domain;

        public SortBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public Sort Build(ISearchRequest request)
        {
            if (request.SortOptions == null || request.SortOptions.Length == 0)
                return new Sort();

            var sortFields = request.SortOptions
                .Select(opt => GetSortField(domain, opt))
                .Where(f => f != null)
                .ToList();

            sortFields.Add(SortField.FIELD_SCORE);
            return new Sort(sortFields.ToArray());
        }

        SortField GetSortField(ISearchDomain domain, ISortOption sortOption)
        {
            var field = domain.Fields.SingleOrDefault(f => f.Name == sortOption.FieldName);
            return field == null ? null : GetSortField(domain, sortOption, field);
        }

        SortField GetSortField(ISearchDomain domain, ISortOption sortOption, IField field)
        {
            switch (field.Type)
            {
                case FieldType.Date:
                case FieldType.DateArray:
                case FieldType.Double:
                case FieldType.DoubleArray:
                case FieldType.Bool:
                case FieldType.BoolArray:
                case FieldType.Int:
                case FieldType.IntArray:
                    return new NumericSortBuilder().Build(domain, sortOption, field);

                case FieldType.Literal:
                case FieldType.LiteralArray:
                    return new LiteralSortBuilder().Build(domain, sortOption, field);

                case FieldType.Text:
                case FieldType.TextArray:
                    return new TextSortBuilder().Build(domain, sortOption, field);

                default:
                case FieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}
