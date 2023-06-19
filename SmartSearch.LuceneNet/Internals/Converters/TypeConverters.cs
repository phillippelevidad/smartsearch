using System;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    internal class BoolConverter
    {
        private static readonly Type boolType = typeof(bool);

        public static bool ConvertFromInt(int value)
        {
            return value == 1 ? true : false;
        }

        public static int ConvertToInt(object value)
        {
            return Convert(value) ? 1 : 0;
        }

        private static bool Convert(object value)
        {
            if (value.GetType() == boolType)
                return (bool)value;

            return System.Convert.ToBoolean(value);
        }
    }

    internal class DateTimeConverter
    {
        private static readonly Type dateTimeType = typeof(DateTime);

        public static DateTime ConvertFromLong(long value)
        {
            return new DateTime(value);
        }

        public static long ConvertToLong(object value)
        {
            return Convert(value).Ticks;
        }

        private static DateTime Convert(object value)
        {
            if (value.GetType() == dateTimeType)
                return (DateTime)value;

            return System.Convert.ToDateTime(value);
        }
    }

    internal class DoubleConverter
    {
        private static readonly Type doubleType = typeof(double);

        public static double Convert(object value)
        {
            if (value.GetType() == doubleType)
                return (double)value;

            return System.Convert.ToDouble(value);
        }
    }

    internal class LongConverter
    {
        private static readonly Type longType = typeof(long);

        public static long Convert(object value)
        {
            if (value.GetType() == longType)
                return (long)value;

            return System.Convert.ToInt64(value);
        }
    }

    internal class StringConverter
    {
        private static readonly Type stringType = typeof(string);

        public static string Convert(object value)
        {
            if (value.GetType() == stringType)
                return (string)value;

            return System.Convert.ToString(value) ?? "";
        }
    }
}
