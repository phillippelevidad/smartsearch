using SmartSearch.Abstractions;
using System;

namespace SmartSearch.LuceneNet
{
    public class ErrorCreatingLuceneIndexException : Exception
    {
        public ErrorCreatingLuceneIndexException(Exception innerException) : base("Error creating Lucene Index.", innerException) { }
    }

    public class UnknownFieldTypeException : Exception
    {
        public UnknownFieldTypeException(FieldType type) : base($"Unknown field type: '{type}'.") { }
    }
}
