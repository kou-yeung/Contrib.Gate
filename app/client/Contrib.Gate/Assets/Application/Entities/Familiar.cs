///============================
/// データ : 使い魔
///============================
using CsvHelper.Configuration;
using Util;

namespace Entities
{
    public class Familiar
    {
        public Identify Identify;
        public string Name;
        public string Image;        // 画像ID
        public Race Race;
        public int Rarity;
        public int[] Attribute;     // 属性
        public int[] baseParam;     // レベル1時のパラメータ
        public int[] additionParam; // レベル1上昇時のパラメータ増加
    }

    public sealed class FamiliarMap : ClassMap<Familiar>
    {
        public FamiliarMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Image).Index(2);
            Map(x => x.Race).Index(3);
            Map(x => x.Rarity).Index(4);
            Map(x => x.Attribute).ConvertUsing(row =>
            {
                var res = new int[4];
                int value;

                // 属性1
                var a1 = row.GetField(5);
                int.TryParse(row.GetField(6), out value);
                if (!string.IsNullOrEmpty(a1))
                {
                    res[(int)EnumExtension<Attribute>.Parse(a1)] = value;
                } 
                // 属性2
                var a2 = row.GetField(7);
                int.TryParse(row.GetField(8), out value);
                if (!string.IsNullOrEmpty(a2))
                {
                    res[(int)EnumExtension<Attribute>.Parse(a2)] = value;
                }
                return res;
            });
            Map(x => x.baseParam).Index(9, 9 + (int)Param.Count - 1);
            Map(x => x.additionParam).Index(17, 17 + (int)Param.Count - 1);
        }
    }
}
