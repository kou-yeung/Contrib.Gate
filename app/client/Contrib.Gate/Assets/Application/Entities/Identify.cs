///==========================
/// データ : 識別
/// 0x(FF)[FFFFFF]
/// (FF) type
/// [FFFFFF] id
///==========================

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
    }
}
