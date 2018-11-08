using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Synonym;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Util;
using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace SmartSearch.LuceneNet.Internals.SpecializedFields
{
    class SynonymsProcessor
    {
        SynonymMap synonymMap;
        readonly InternalSearchDomain domain;

        SynonymsProcessor(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public static SynonymsProcessor Initialize(InternalSearchDomain domain)
        {
            var processor = new SynonymsProcessor(domain);
            processor.BuildSynonymMap();
            return processor;
        }

        public object Process(ISpecializedField field, object value)
        {
            if (!field.IsString())
                return new KeyValuePair<string, object>(field.Name, value);

            return field.IsArray()
                ? ProcessStringArray(value as Array)
                : ProcessString(value as string);
        }

        object ProcessString(string value)
        {
            if (value == null)
                return null;

            using (var reader = new StringReader(value))
            {
                var version = Definitions.LuceneVersion;
                TokenStream stream = new StandardTokenizer(version, reader);

                stream = new LowerCaseFilter(version, stream);
                stream = new StandardFilter(version, stream);
                stream = new SynonymFilter(stream, synonymMap, true);

                return stream.GetContent();
            }
        }

        object ProcessStringArray(Array array)
        {
            var results = new string[array.Length];

            for (int i = 0; i < array.Length; i++)
                results[i] = array.GetValue(i) as string;

            return results;
        }

        void BuildSynonymMap()
        {
            var builder = new SynonymMap.Builder(true);

            if (domain.AnalysisSettings != null && domain.AnalysisSettings.Synonyms != null)
            {
                foreach (var group in domain.AnalysisSettings.Synonyms)
                    foreach (var item in group)
                        foreach (var other in group)
                            if (item != other)
                                builder.Add(new CharsRef(item), new CharsRef(other), true);
            }

            synonymMap = builder.Build();
        }
    }

    static class TokenStreamExtensions
    {
        public static string GetContent(this TokenStream stream)
        {
            var offsetAttribute = new OffsetAttribute();
            var charTermAttribute = new CharTermAttribute();
            var terms = new List<string>();

            stream.AddAttributeImpl(offsetAttribute);
            stream.AddAttributeImpl(charTermAttribute);
            stream.Reset();

            while (stream.IncrementToken())
            {
                int start = offsetAttribute.StartOffset;
                int end = offsetAttribute.EndOffset;
                terms.Add(charTermAttribute.ToString());
            }

            return string.Join(" ", terms);
        }
    }
}
