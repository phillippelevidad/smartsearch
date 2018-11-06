namespace SmartSearch.Abstractions
{
    public interface IField
    {
        string Name { get; }

        FieldType Type { get; }

        bool EnableFaceting { get; }

        bool EnableSearching { get; }

        bool EnableReturning { get; }

        bool EnableSorting { get; }
    }

    public enum FieldType
    {
        Date, DateArray, Double, DoubleArray, Int, IntArray, LatLng, Literal, LiteralArray, Text, TextArray
    }
}
