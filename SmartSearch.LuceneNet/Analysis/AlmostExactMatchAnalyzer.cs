using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using System.IO;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Pattern;
using System.Text.RegularExpressions;

namespace SmartSearch.LuceneNet.Analysis
{
    internal class AlmostExactMatchAnalyzer : Analyzer
    {
        protected override TokenStreamComponents CreateComponents(
            string fieldName,
            TextReader reader
        )
        {
            Tokenizer source = new StandardTokenizer(LuceneVersion.LUCENE_48, reader);
            TokenStream filter = new LowerCaseFilter(LuceneVersion.LUCENE_48, source);
            filter = new ASCIIFoldingFilter(filter);
            filter = new PatternReplaceFilter(filter, new Regex(@"\p{P}"), "", true);
            filter = new SingularPortugueseFilter(filter);
            return new TokenStreamComponents(source, filter);
        }
    }
}
