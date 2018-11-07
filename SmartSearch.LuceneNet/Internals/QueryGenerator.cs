using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals
{
    static class QueryGenerator
    {
        public static Query GetQuery(ISearchDomain domain, ISearchRequest request, IAnalyzerFactory analyzerFactory)
        {
            var analyzer = analyzerFactory.Create();
            var parser = new QueryParser(Definitions.LuceneVersion, "", analyzer);
            var queryExpression = QueryExpressionGenerator.GetQueryExpression(domain, request);
            return GetQueryInternal(parser, queryExpression);
        }

        static Query GetQueryInternal(QueryParser parser, string queryExpression)
        {
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

        static string FixQueryExpression(string queryExpression, ParseException parseEx)
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
