///==========================
/// データ : 識別
/// 0x(FF)[FFFFFF]
/// (FF) type
/// [FFFFFF] id
///==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Text.RegularExpressions;
using System;
using Util;

namespace Entities
{
    public class Identify
    {
        public uint idWithType { get; private set; }

        public IDType Type
        {
            get { return (IDType)((idWithType >> 24) & 0xFF); }
        }
        public uint Id
        {
            get { return idWithType & 0xFFFFFF; }
        }

        public Identify(uint idWithType)
        {
            this.idWithType = idWithType;
        }

        public Identify(IDType type, uint id)
        {
            idWithType = ((uint)type) << 24 | id;
        }

        public override string ToString()
        {
            return $"{Type}_{Id / 1000:D3}_{Id % 1000:D3}";
        }
    }


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
