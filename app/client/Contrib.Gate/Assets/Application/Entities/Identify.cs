///==========================
/// データ : 識別
/// 0x(FF)[FFFFFF]
/// (FF) type
/// [FFFFFF] id
///==========================
using System.Text.RegularExpressions;
using Util;

namespace Entities
{
    public class Identify
    {
        public static readonly Identify Empty = new Identify(0);

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

        public override bool Equals(object obj)
        {
            if (obj is Identify)
            {
                return idWithType == (obj as Identify).idWithType;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return idWithType.GetHashCode();
        }

        public static bool operator ==(Identify a, Identify b)
        {
            return a.idWithType == b.idWithType;
        }
        public static bool operator !=(Identify a, Identify b)
        {
            return !(a == b);
        }

        public static implicit operator uint(Identify d)
        {
            return d.idWithType;
        }
        public static implicit operator Identify(uint idWithType)
        {
            return new Identify(idWithType);
        }


        /// <summary>
        /// 文字列 -> Identify
        /// </summary>
        static readonly string pattern = @"(\w+?)_(\d+)_(\d+)";
        static Regex regex = new Regex(pattern);
        public static Identify Parse(string text)
        {
            var m = regex.Match(text);
            if (m == Match.Empty) return new Identify(0);

            var type = EnumExtension<IDType>.Parse(m.Groups[1].ToString());
            var id = uint.Parse(m.Groups[2].ToString() + m.Groups[3].ToString());
            return new Identify(type, id);
        }
    }
}
