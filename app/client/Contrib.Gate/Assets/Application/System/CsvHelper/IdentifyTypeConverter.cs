using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;
using Util;
using Entities;

namespace CsvHelper
{
    public class IdentifyTypeConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return Identify.Parse(text);
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return (value as Identify).ToString();
        }
    }

}
