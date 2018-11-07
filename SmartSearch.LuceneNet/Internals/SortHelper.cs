using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals
{
    static class SortHelper
    {
        public static Sort GetSort(ISearchDomain domain, ISearchRequest request)
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

        static SortField GetSortField(ISearchDomain domain, ISortOption sortOption)
        {
            var field = domain.Fields.SingleOrDefault(f => f.Name == sortOption.FieldName);

            if (field == null)
                return null;

            var type = GetSortFieldType(field);
            var sortDescending = sortOption.Direction == SortDirection.Descending;
            return new SortField(field.Name, type, sortDescending);
        }

        static SortFieldType GetSortFieldType(IField field)
        {
            switch (field.Type)
            {
                case FieldType.Date:
                case FieldType.DateArray:
                    return SortFieldType.INT64;

                case FieldType.Double:
                case FieldType.DoubleArray:
                    return SortFieldType.DOUBLE;

                case FieldType.Int:
                case FieldType.IntArray:
                    return SortFieldType.INT32;

                case FieldType.Literal:
                case FieldType.LiteralArray:
                case FieldType.Text:
                case FieldType.TextArray:
                    return SortFieldType.STRING;

                default:
                case FieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }
    }
}
