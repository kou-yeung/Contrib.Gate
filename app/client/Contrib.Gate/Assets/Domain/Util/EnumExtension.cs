using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Util
{
    public static class EnumExtension<T> where T : struct, IConvertible
    {
        static string[] Keys = Enum.GetNames(typeof(T));
        static T[] Values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();

        /// <summary>
        /// Enum => string
        /// </summary>
        public static string ToString(T e)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i].Equals(e)) return Keys[i];
            }
            return null;
        }

        /// <summary>
        /// string => Enum
        /// </summary>
        public static T Parse(string text)
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i].Equals(text)) return Values[i];
            }
            return default(T);
        }
    }

    public class EnumTypeConverter<T> : ITypeConverter where T : struct, IConvertible
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return EnumExtension<T>.Parse(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return EnumExtension<T>.ToString((T)value);
        }
    }
}
