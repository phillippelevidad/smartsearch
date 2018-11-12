using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Search;
using Lucene.Net.Util;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Internals.Converters;
using System.Linq;
using System.Collections.Generic;
using Lucene.Net.Queries;

namespace SmartSearch.LuceneNet.Internals.Builders
{
    class FilterBuilder
    {
        readonly InternalSearchDomain domain;

        public FilterBuilder(InternalSearchDomain domain)
        {
            this.domain = domain;
        }

        public Filter Build(ISearchRequest request, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (request.Filters == null || request.Filters.Length == 0)
                return null;

            var booleanFilter = new BooleanFilter();

            foreach (var requestFilter in request.Filters)
            {
                var field = domain.Fields.Single(f => f.Name == requestFilter.FieldName);
                var filter = BuildFilterForField(request, requestFilter, field, perFieldAnalyzer);

                if (filter != null)
                    booleanFilter.Add(filter, Occur.MUST);
            }

            return booleanFilter;
        }

        Filter BuildFilterForField(ISearchRequest request, IQueryFilter requestFilter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            if (requestFilter.Value == null)
                return null;

            switch (field.Type)
            {
                case FieldType.Bool:
                case FieldType.BoolArray:
                    var internalBoolValue = BoolConverter.ConvertToInt(requestFilter.Value);
                    return NumericRangeFilter.NewInt32Range(field.Name, internalBoolValue, internalBoolValue, true, true);

                case FieldType.Date:
                case FieldType.DateArray:
                    var internalDateValue = DateTimeConverter.ConvertToLong(requestFilter.Value);
                    return NumericRangeFilter.NewInt64Range(field.Name, internalDateValue, internalDateValue, true, true);

                case FieldType.Double:
                case FieldType.DoubleArray:
                    var internalDoubleValue = DoubleConverter.Convert(requestFilter.Value);
                    return NumericRangeFilter.NewDoubleRange(field.Name, internalDoubleValue, internalDoubleValue, true, true);

                case FieldType.Int:
                case FieldType.IntArray:
                    var internalLongValue = LongConverter.Convert(requestFilter.Value);
                    return NumericRangeFilter.NewInt64Range(field.Name, internalLongValue, internalLongValue, true, true);

                case FieldType.Literal:
                case FieldType.LiteralArray:
                case FieldType.Text:
                case FieldType.TextArray:
                    return BuildFilterForTextField(request, requestFilter, field, perFieldAnalyzer);

                default:
                case FieldType.LatLng:
                    throw new UnknownFieldTypeException(field.Type);
            }
        }

        Filter BuildFilterForTextField(ISearchRequest request, IQueryFilter requestFilter, IField field, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            var value = StringConverter.Convert(requestFilter.Value);
            var terms = ExtractTermsFromString(field.Name, value, perFieldAnalyzer);
            return new FieldCacheTermsFilter(field.Name, terms);
        }

        string[] ExtractTermsFromString(string fieldName, string value, PerFieldAnalyzerWrapper perFieldAnalyzer)
        {
            TokenStream tokenStream = null;

            try
            {
                tokenStream = perFieldAnalyzer.GetTokenStream(fieldName, value);

                var offsetAttribute = new OffsetAttribute();
                var charTermAttribute = new CharTermAttribute();
                var terms = new List<string>();

                tokenStream.AddAttributeImpl(offsetAttribute);
                tokenStream.AddAttributeImpl(charTermAttribute);
                tokenStream.Reset();

                while (tokenStream.IncrementToken())
                {
                    int start = offsetAttribute.StartOffset;
                    int end = offsetAttribute.EndOffset;
                    terms.Add(charTermAttribute.ToString());
                }

                return terms.ToArray();
            }
            finally
            {
                if (tokenStream != null)
                    IOUtils.DisposeWhileHandlingException(tokenStream);
            }
        }
    }
}
