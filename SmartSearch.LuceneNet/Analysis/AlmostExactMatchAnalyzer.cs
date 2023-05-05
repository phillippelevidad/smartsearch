using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using System.IO;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Pattern;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis.TokenAttributes;
using System;

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

            // Replaces diacritics, e.g. "é" to "e".
            filter = new ASCIIFoldingFilter(filter);

            // Removes punctuation.
            filter = new PatternReplaceFilter(filter, new Regex(@"\p{P}"), "", true);

            // Changes plural words to their singular forms.
            filter = new SingularPortugueseFilter(filter);

            return new TokenStreamComponents(source, filter);
        }

        private class SingularPortugueseFilter : TokenFilter
        {
            private readonly ICharTermAttribute termAttr;

            public SingularPortugueseFilter(TokenStream input) : base(input)
            {
                termAttr = AddAttribute<ICharTermAttribute>();
            }

            public sealed override bool IncrementToken()
            {
                // We don't use PortugueseStemmer on purpose, because it would stem words
                // instead of just changing their endings to the proper singular form.

                if (!m_input.IncrementToken())
                    return false;

                string term = termAttr.ToString();

                // Handle plural forms ending in "ões" to end in "ão"
                term = HandleEndings(term, new[] { "oes", "ões" }, t => t.Substring(0, t.Length - 2) + "ao");

                // Handle plural forms ending in "ães" to end in "ãe"
                term = HandleEndings(term, new[] { "aes", "ães" }, t => t.Substring(0, t.Length - 2) + "ae");

                // Handle plural forms ending in "os" to end in "o"
                term = HandleEndings(term, new[] { "os" }, t => t.Substring(0, t.Length - 1));

                // Handle plural forms ending in "as" to end in "a"
                term = HandleEndings(term, new[] { "as" }, t => t.Substring(0, t.Length - 1));

                // Handle plural forms ending in "zes" to end in "z" (e.g. "cicatrizes")
                term = HandleEndings(term, new[] { "zes" }, t => t.Substring(0, t.Length - 2));

                // Handle plural forms ending in "res" to end in "r" (e.g. "prazers")
                term = HandleEndings(term, new[] { "res" }, t => t.Substring(0, t.Length - 2));

                termAttr.SetEmpty().Append(term);

                return true;
            }

            private string HandleEndings(string term, string[] endings, Func<string, string> handler)
            {
                foreach (var ending in endings)
                {
                    if (term.EndsWith(ending, StringComparison.InvariantCultureIgnoreCase))
                    {
                        term = handler(term);
                    }
                }

                return term;
            }
        }
    }
}
