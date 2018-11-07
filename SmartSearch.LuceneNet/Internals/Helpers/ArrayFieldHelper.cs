using SmartSearch.Abstractions;

namespace SmartSearch.LuceneNet.Internals.Helpers
{
    static class ArrayFieldHelper
    {
        public static bool IsArrayField(IField field)
        {
            switch (field.Type)
            {
                case FieldType.DateArray:
                case FieldType.DoubleArray:
                case FieldType.IntArray:
                case FieldType.LiteralArray:
                case FieldType.TextArray:
                    return true;
                default:
                    return false;
            }
        }
    }
}
