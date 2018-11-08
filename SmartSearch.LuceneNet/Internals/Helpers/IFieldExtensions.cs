namespace SmartSearch.Abstractions
{
    static class IFieldExtensions
    {
        public static bool IsArray(this IField field)
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

        public static bool IsString(this IField field)
        {
            switch (field.Type)
            {
                case FieldType.Literal:
                case FieldType.LiteralArray:
                case FieldType.Text:
                case FieldType.TextArray:
                    return true;
                default:
                    return false;
            }
        }
    }
}
