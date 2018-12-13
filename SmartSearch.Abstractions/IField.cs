namespace SmartSearch.Abstractions
{
    public enum FieldRelevance
    {
        Normal, High, Higher
    }

    public enum FieldType
    {
        Bool, BoolArray,
        Date, DateArray,
        Double, DoubleArray,
        Int, IntArray,
        LatLng,
        Literal, LiteralArray,
        Text, TextArray
    }

    public interface IField
    {
        bool EnableFaceting { get; }
        bool EnableSearching { get; }
        bool EnableSorting { get; }
        string Name { get; }

        FieldRelevance Relevance { get; }

        FieldType Type { get; }
    }
}