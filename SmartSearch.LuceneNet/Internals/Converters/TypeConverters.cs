using System;
using System.ComponentModel;

namespace SmartSearch.LuceneNet.Internals.Converters
{
    class DateTimeConverter
    {
        static readonly Type dateTimeType = typeof(DateTime);

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
