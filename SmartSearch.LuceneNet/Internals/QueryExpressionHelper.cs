using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals
{
    static class QueryExpressionHelper
    {
        public static string GetQueryExpression(ISearchDomain domain, ISearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return "";

            if (Definitions.MatchAllDocsQuery.Equals(request.Query, StringComparison.OrdinalIgnoreCase))
                return "*:";

            var filter = BuildFilterExpression(domain, request);
            var search = BuildSearchExpression(domain, request);

            return string.Join(" ", filter, search);
        }

        static string BuildFilterExpression(ISearchDomain domain, ISearchRequest request)
        {
            if (request.Filters == null || request.Filters.Length == 0)
                return "";

            var expressions = new List<string>(request.Filters.Length);

            foreach (var f in request.Filters)
                expressions.Add($"+({f.FieldName}:{f.Value})");

            return string.Join(" ", expressions);
        }

        static string BuildSearchExpression(ISearchDomain domain, ISearchRequest request)
        {
            var words = request.Query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var searchFields = domain.Fields
                .Where(f => f.EnableSearching)
                .Where(f => !request.Filters.Any(x => f.Name.Equals(x.FieldName, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            var expressions = new List<string>(searchFields.Length);

            foreach (var field in searchFields)
            {
                var allWords = string.Join(" ", words);
                expressions.Add($"{field.Name}:({allWords})");
            }

            return string.Join(" ", expressions);
        }
    }
}
