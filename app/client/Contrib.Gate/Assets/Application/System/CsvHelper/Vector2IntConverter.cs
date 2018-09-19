using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CsvHelper
{
    public class Vector2IntTypeConverter : ITypeConverter
    {
        static readonly string pattern = @"(\d+)x(\d+)";
        static Regex regex = new Regex(pattern);

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var m = regex.Match(text);
            if (m == Match.Empty) return Vector2Int.zero;

            var x = int.Parse(m.Groups[1].ToString());
            var y = int.Parse(m.Groups[2].ToString());
            return new Vector2Int(x, y);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            var v = (Vector2Int)value;
            return $"{v.x}x{v.y}";
        }
    }

}
