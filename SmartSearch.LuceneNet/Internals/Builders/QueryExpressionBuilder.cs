using Lucene.Net.Search;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    internal class QueryExpressionBuilder
    {
        private readonly InternalSearchDomain domain;

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

            return BuildInternal(request);
        }

        private string BuildInternal(ISearchRequest request)
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
                {
                    if (field.RelevanceBoost == 1f)
                    {
                        fieldExpressions.Add($"{field.Name}:{word}");

                    }
                    else
                    {
                        var boostExpression = field.RelevanceBoost.ToString("0.0", CultureInfo.InvariantCulture);
                        fieldExpressions.Add($"({field.Name}:{word})^{boostExpression}");
                    }
                }

                var expression = string.Join(" ", fieldExpressions);
                wordExpressions.Add($"+({expression})");
            }

            return string.Join(" ", wordExpressions);
        }
    }
}