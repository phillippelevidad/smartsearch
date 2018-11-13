using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class QueryBuilder
    {
        readonly InternalSearchDomain domain;

        public QueryBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public Query Build(ISearchRequest request, Analyzer analyzer)
        {
            var parser = new QueryParser(Definitions.LuceneVersion, "", analyzer);
            var queryExpression = new QueryExpressionBuilder(domain).Build(request);
            var filter = new FilterBuilder(domain).Build(request, analyzer as PerFieldAnalyzerWrapper);
            var query = BuildInternal(parser, queryExpression);

            return filter == null ? query : new FilteredQuery(query, filter);
        }

        Query BuildInternal(QueryParser parser, string queryExpression)
        {
            if (string.IsNullOrEmpty(queryExpression))
                return new MatchAllDocsQuery();

            while (true)
            {
                try
                {
                    return parser.Parse(queryExpression);
                }
                catch (ParseException parseEx)
                {
                    queryExpression = FixQueryExpression(queryExpression, parseEx);
                }
            }
        }

        string FixQueryExpression(string queryExpression, ParseException parseEx)
        {
            if (parseEx.CurrentToken == null)
            {
                parseEx = parseEx.InnerException as ParseException;
                if (parseEx == null || parseEx.CurrentToken == null)
                    return "";
            }

            var errorToken = parseEx.CurrentToken.Next;
            if (errorToken.Image.Length == 0)
                errorToken = parseEx.CurrentToken;

            return
                queryExpression.Substring(0, errorToken.BeginColumn) +
                queryExpression.Substring(errorToken.EndColumn);
        }
    }
}
