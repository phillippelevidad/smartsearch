using Lucene.Net.Util;

namespace SmartSearch.LuceneNet.Internals
{
    static class Definitions
    {
        public static readonly string DocumentIdFieldName = "__docid";

        public static readonly LuceneVersion LuceneVersion = LuceneVersion.LUCENE_48;

        public static readonly string MatchAllDocsQuery = "matchAllDocs";
    }
}
