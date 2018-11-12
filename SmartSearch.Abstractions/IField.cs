namespace SmartSearch.Abstractions
{
    public interface IField
    {
        string Name { get; }

        FieldRelevance Relevance { get; }

        FieldType Type { get; }

        bool EnableFaceting { get; }

        bool EnableSearching { get; }

        bool EnableSorting { get; }
    }

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
}
