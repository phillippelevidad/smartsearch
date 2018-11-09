using Lucene.Net.Support;
using Lucene.Net.Util;
using System.Text.RegularExpressions;

namespace Lucene.Net.Analysis.Synonym
{
    // https://stackoverflow.com/a/47278296/484108
    // https://github.com/apache/lucenenet/blob/a3a12967b250e8e7e5f623f0ba7572ec64f479ac/src/Lucene.Net.Tests.Analysis.Common/Analysis/Synonym/TestSynonymMapFilter.cs#L1168-L1178
    static class SynonymMapBuilderExtensions
    {
        static Regex space = new Regex(" +", RegexOptions.Compiled);

        public static void Add(this SynonymMap.Builder builder, string input, string output, bool keepOriginal = true)
        {
            var inputCharsRef = new CharsRef();
            SynonymMap.Builder.Join(space.Split(input).TrimEnd(), inputCharsRef);

            var outputCharsRef = new CharsRef();
            SynonymMap.Builder.Join(space.Split(output).TrimEnd(), outputCharsRef);

            builder.Add(inputCharsRef, outputCharsRef, keepOriginal);
        }
    }
}
