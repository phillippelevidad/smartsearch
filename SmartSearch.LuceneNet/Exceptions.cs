using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet
{
    public class DuplicatedFieldException : Exception
    {
        public DuplicatedFieldException(string fieldName)
            : base($"Field '{fieldName}' occurs twice in a single search domain.") { }
    }

    public class ErrorCreatingLuceneIndexException : Exception
    {
        public ErrorCreatingLuceneIndexException(Exception innerException)
            : base("Error creating Lucene Index.", innerException) { }
    }

    public class InvalidFieldNameException : Exception
    {
        public InvalidFieldNameException(string fieldName)
            : base($"Invalid name for field: '{fieldName}'. A valid name should be comprised only of alphanumeric characteres and underscores.") { }
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

    public class InvalidLatLngFilterValue : Exception
    {
        public InvalidLatLngFilterValue(string fieldName)
            : base($"Invalid filter value for field '{fieldName}'. Use an implementation of '{typeof(ILatLngFilterValue).FullName}'.") { }
    }

    public class InvalidSearchDomainNameException : Exception
    {
        public InvalidSearchDomainNameException(string domainName)
            : base($"Invalid name for search domain: '{domainName}'. A valid name should be comprised only of alphanumeric characteres and underscores.") { }
    }

    public class LatLngFieldValueMustImplementIGeoCoordinateException : Exception
    {
        public LatLngFieldValueMustImplementIGeoCoordinateException(string fieldName)
            : base($"Field '{fieldName}' must implement {typeof(ILatLng).FullName}.") { }
    }

    public class MissingLatLngReferenceForSortingException : Exception
    {
        public MissingLatLngReferenceForSortingException()
            : base("Must provide a LatLng reference when sorting by a LatLng field.") { }
    }

    public class RangeFilterNotSupportedForLatLngFieldsException : Exception
    {
        public RangeFilterNotSupportedForLatLngFieldsException(string fieldName)
            : base($"Range filter is not supported for LatLng fields. Attempted field: '{fieldName}'.") { }
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