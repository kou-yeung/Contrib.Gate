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
            Map(x => x.Attribute).Index(5, 5 + (int)Attribute.Count - 1);
            Map(x => x.baseParam).Index(9, 9 + (int)Param.Count - 1);
            Map(x => x.additionParam).Index(17, 17 + (int)Param.Count - 1);
        }
    }
}
