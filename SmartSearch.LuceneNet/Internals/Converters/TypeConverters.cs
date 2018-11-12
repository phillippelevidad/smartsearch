using System;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    class BoolConverter
    {
        static readonly Type boolType = typeof(bool);

        public static bool ConvertFromInt(int value)
        {
            return value == 1 ? true : false;
        }

        public static int ConvertToInt(object value)
        {
            return Convert(value) ? 1 : 0;
        }

        static bool Convert(object value)
        {
            if (value.GetType() == boolType)
                return (bool)value;

            return System.Convert.ToBoolean(value);
        }
    }

    class DateTimeConverter
    {
        static readonly Type dateTimeType = typeof(DateTime);

        public static DateTime ConvertFromLong(long value)
        {
            return new DateTime(value);
        }

        public static long ConvertToLong(object value)
        {
            return Convert(value).Ticks;
        }

        static DateTime Convert(object value)
        {
            if (value.GetType() == dateTimeType)
                return (DateTime)value; ;

            return System.Convert.ToDateTime(value);
        }
    }

    class DoubleConverter
    {
        static readonly Type doubleType = typeof(double);

        public static double Convert(object value)
        {
            if (value.GetType() == doubleType)
                return (double)value; ;

            return System.Convert.ToDouble(value);
        }
    }

    class LongConverter
    {
        static readonly Type longType = typeof(long);

        public static long Convert(object value)
        {
            if (value.GetType() == longType)
                return (long)value; ;

            return System.Convert.ToInt64(value);
        }
    }

    class StringConverter
    {
        static readonly Type stringType = typeof(string);

        public static string Convert(object value)
        {
            if (value.GetType() == stringType)
                return (string)value; ;

            return System.Convert.ToString(value);
        }
    }
}
