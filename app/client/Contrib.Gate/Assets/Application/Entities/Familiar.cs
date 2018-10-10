///============================
/// データ : 使い魔
///============================
using CsvHelper.Configuration;

namespace Entities
{
    public class Familiar
    {
        public Identify Identify;
        public string Name;
        public string Image;        // 画像ID
        public Race Race;
        public int Rarity;
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
            Map(x => x.baseParam).Index(5, 5 + (int)Param.Count - 1);
            Map(x => x.additionParam).Index(13, 13 + (int)Param.Count - 1);
        }
    }
}
