using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet
{
    public class InvalidIndexContextTypeException : Exception
    {
        public InvalidIndexContextTypeException(Type type) : base($"Type '{type?.FullName}' is not valid as an IndexContext.") { }
    }

    public class ErrorCreatingLuceneIndexException : Exception
    {
        public ErrorCreatingLuceneIndexException(Exception innerException) : base("Error creating Lucene Index.", innerException) { }
    }

    public class UnknownFieldRelevanceException : Exception
    {
        public UnknownFieldRelevanceException(FieldRelevance relevance) : base($"Unknown field relevance: '{relevance}'.") { }
    }

    public class UnknownFieldTypeException : Exception
    {
        public UnknownFieldTypeException(FieldType type) : base($"Unknown field type: '{type}'.") { }
    }

    public class InvalidFilterTypeForFilterBuilderException : Exception
    {
        public InvalidFilterTypeForFilterBuilderException(Type filterBuilderType, FieldType fieldType)
            : base($"Field type '{fieldType}' is not valid for builder of type '{filterBuilderType?.Name}'.") { }
    }

    public class RangeFilterNotSupportedForTextAndLiteralFieldsException : Exception
    {
        public RangeFilterNotSupportedForTextAndLiteralFieldsException(string fieldName)
            :base ($"Range filter is not supported for Text and Literal fields. Attempted field: '{fieldName}'.") { }
    }
}
