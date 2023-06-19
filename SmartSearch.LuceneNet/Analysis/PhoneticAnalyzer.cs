using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Util;
using System.IO;
using System.Text;

namespace SmartSearch.LuceneNet.Analysis
{
    internal class PhoneticAnalyzer : Analyzer
    {
        protected override TokenStreamComponents CreateComponents(
            string fieldName,
            TextReader reader
        )
        {
            Tokenizer tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, reader);

            TokenStream tokenStream = new StandardFilter(LuceneVersion.LUCENE_48, tokenizer);
            tokenStream = new LowerCaseFilter(LuceneVersion.LUCENE_48, tokenStream);
            tokenStream = new ASCIIFoldingFilter(tokenStream);
            tokenStream = new PhoneticFilter(tokenStream);

            return new TokenStreamComponents(tokenizer, tokenStream);
        }

        private sealed class PhoneticFilter : TokenFilter
        {
            private readonly ICharTermAttribute termAttribute;

            public PhoneticFilter(TokenStream input) : base(input)
            {
                termAttribute = AddAttribute<ICharTermAttribute>();
            }

            public override bool IncrementToken()
            {
                if (m_input.IncrementToken())
                {
                    string term = termAttribute.ToString();
                    string phoneticTerm = SoundexEncode(term);

                    termAttribute.SetEmpty().Append(phoneticTerm);
                    return true;
                }

                return false;
            }

            private string SoundexEncode(string term)
            {
                StringBuilder encodedTerm = new StringBuilder();

                // Convert the term to uppercase and remove non-alphabetic characters
                term = term.ToUpper();
                foreach (char c in term)
                {
                    if (char.IsLetter(c))
                    {
                        encodedTerm.Append(c);
                    }
                }

                if (encodedTerm.Length == 0)
                    return "";

                // Apply Soundex encoding rules
                char[] encodedChars = new char[4];
                encodedChars[0] = encodedTerm[0];

                int count = 1;
                for (int i = 1; i < encodedTerm.Length && count < 4; i++)
                {
                    char encodedChar = SoundexMap(encodedTerm[i]);
                    if (encodedChar != encodedChars[count - 1])
                    {
                        encodedChars[count] = encodedChar;
                        count++;
                    }
                }

                while (count < 4)
                {
                    encodedChars[count] = '0';
                    count++;
                }

                return new string(encodedChars);
            }

            private char SoundexMap(char c)
            {
                switch (c)
                {
                    case 'B':
                    case 'F':
                    case 'P':
                    case 'V':
                        return '1';
                    case 'C':
                    case 'G':
                    case 'J':
                    case 'K':
                    case 'Q':
                    case 'S':
                    case 'X':
                    case 'Z':
                        return '2';
                    case 'D':
                    case 'T':
                        return '3';
                    case 'L':
                        return '4';
                    case 'M':
                    case 'N':
                        return '5';
                    case 'R':
                        return '6';
                    default:
                        return '0';
                }
            }
        }
    }
}
