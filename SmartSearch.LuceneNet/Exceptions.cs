﻿using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet
{
    public class ErrorCreatingLuceneIndexException : Exception
    {
        public ErrorCreatingLuceneIndexException(Exception innerException)
            : base("Error creating Lucene Index.", innerException) { }
    }

    public class InvalidFilterTypeForFilterBuilderException : Exception
    {
        public InvalidFilterTypeForFilterBuilderException(Type filterBuilderType, FieldType fieldType)
            : base($"Field type '{fieldType}' is not valid for builder of type '{filterBuilderType?.Name}'.") { }
    }

    public class InvalidIndexContextTypeException : Exception
    {
        public InvalidIndexContextTypeException(Type type)
            : base($"Type '{type?.FullName}' is not valid as an IndexContext.") { }
    }

    public class RangeFilterNotSupportedForTextAndLiteralFieldsException : Exception
    {
        public RangeFilterNotSupportedForTextAndLiteralFieldsException(string fieldName)
            : base($"Range filter is not supported for Text and Literal fields. Attempted field: '{fieldName}'.") { }
    }

    public class SearchDomainDoesNotSpecifySearchEnabledFieldsException : Exception
    {
        public SearchDomainDoesNotSpecifySearchEnabledFieldsException(string domainName)
            : base($"Search domain '{domainName}' does not specify any search-enabled fields.") { }
    }

    public class UnknownDocumentOperationTypeException : Exception
    {
        public UnknownDocumentOperationTypeException(DocumentOperationType operationType)
            : base($"Unknown document operation type: '{operationType}'.") { }
    }

    public class UnknownFieldRelevanceException : Exception
    {
        public UnknownFieldRelevanceException(FieldRelevance relevance)
            : base($"Unknown field relevance: '{relevance}'.") { }
    }

    public class UnknownFieldTypeException : Exception
    {
        public UnknownFieldTypeException(FieldType type)
            : base($"Unknown field type: '{type}'.") { }
    }
}
