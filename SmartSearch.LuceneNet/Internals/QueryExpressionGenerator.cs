using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartSearch.LuceneNet.Internals
{
    static class QueryExpressionGenerator
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
                .ToArray();

            var wordExpressions = new List<string>(words.Length);

            foreach (var word in words)
            {
                var fieldExpressions = new List<string>(searchFields.Length);

                foreach (var field in searchFields)
                    fieldExpressions.Add($"{field.Name}:{word}");

                var expression = string.Join(" ", fieldExpressions);
                wordExpressions.Add($"+({expression})");
            }

            return string.Join(" ", wordExpressions);
        }
    }
}
