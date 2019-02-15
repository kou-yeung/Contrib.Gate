using CsvHelper.Configuration;

namespace Entities
{
    public class Skill
    {
        public Identify Identify;   // ID
        public string Name;         // スキル名
        public int Level;           // レベル
        public int Cost;            // 消費MP
        public SkillType Type;      // スキル種類
        //public int Turn;            // ターン数
        public SkiiTarget Target;   // 使用対象
        public Param Param;         // パラメータ
        public int[] Attribute;     // 属性
        //public float Coefficient;   // 係数
        public string Script;       // ロジックスクリプト
        public string ActionEffect; // 行動エフェクト
        public string ReceiveEffect;// 受けエフェクト
    }

    public sealed class SkillMap : ClassMap<Skill>
    {
        public SkillMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Level).Index(2);
            Map(x => x.Cost).Index(3);
            Map(x => x.Type).Index(4);
            //Map(x => x.Turn).Index(index++);
            Map(x => x.Target).Index(5);
            Map(x => x.Param).Index(6);
            Map(x => x.Attribute).Index(7, 7 + (int)Attribute.Count - 1);
            //Map(x => x.Coefficient).Index(index++);
            Map(x => x.Script).Index(11);
            Map(x => x.ActionEffect).Index(12);
            Map(x => x.ReceiveEffect).Index(13);
        }
    }
}
