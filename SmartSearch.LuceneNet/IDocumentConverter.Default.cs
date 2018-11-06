using SmartSearch.Abstractions;
using SourceFieldType = SmartSearch.Abstractions.FieldType;

namespace SmartSearch.LuceneNet
{
    public partial class DefaultDocumentConverter : IDocumentConverter
    {
        bool IsArrayField(IField field)
        {
            switch (field.Type)
            {
                case SourceFieldType.DateArray:
                case SourceFieldType.DoubleArray:
                case SourceFieldType.IntArray:
                case SourceFieldType.LiteralArray:
                case SourceFieldType.TextArray:
                    return true;
                default:
                    return false;
            }
        }
    }
}