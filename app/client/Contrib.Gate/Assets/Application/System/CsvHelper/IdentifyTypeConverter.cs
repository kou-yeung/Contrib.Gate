using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;
using Util;
using Entities;

namespace CsvHelper
{
    public class IdentifyTypeConverter : ITypeConverter
    {
        static readonly string pattern = @"(\w+?)_(\d+)_(\d+)";
        static Regex regex = new Regex(pattern);

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var m = regex.Match(text);
            if (m == Match.Empty) return new Identify(0);

            var type = EnumExtension<IDType>.Parse(m.Groups[1].ToString());
            var id = uint.Parse(m.Groups[2].ToString() + m.Groups[3].ToString());
            return new Identify(type, id);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return (value as Identify).ToString();
        }
    }

}
