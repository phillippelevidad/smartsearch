using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class QueryExpressionBuilder
    {
        readonly InternalSearchDomain domain;

        public QueryExpressionBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public string Build(ISearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Query))
                return "";

            if (Definitions.MatchAllDocsQuery.Equals(request.Query, StringComparison.OrdinalIgnoreCase))
                return new MatchAllDocsQuery().ToString();

            var filter = BuildFilterExpression(request);
            var search = BuildSearchExpression(request);

            return string.Join(" ", filter, search);
        }

        string BuildFilterExpression(ISearchRequest request)
        {
            if (request.Filters == null || request.Filters.Length == 0)
                return "";

            var expressions = new List<string>(request.Filters.Length);

            foreach (var f in request.Filters)
                expressions.Add($"+({f.FieldName}:{f.SingleValue})");

            return string.Join(" ", expressions);
        }

        string BuildSearchExpression(ISearchRequest request)
        {
            var searchFields = domain.GetSearchEnabledFields();

            if (request.Query != null && searchFields.Length == 0)
                throw new SearchDomainDoesNotSpecifySearchEnabledFieldsException(domain.Name);

            var words = request.Query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
