using System;
using System.Data;

namespace ConsoleApp
{
    public static class IDataReaderExtensions
    {
        public static T Get<T>(this IDataReader reader, string name)
        {
            var value = reader[name];
            if (value is DBNull) value = null;
            return (T)value;
        }
    }
}