using CsvHelper.Configuration;

namespace Entities
{
    public class Skill
    {
        public Identify Identify;   // ID
        public string Name;         // スキル名
        public int Cost;            // 消費MP
        public SkillType Type;      // スキル種類
        public int Turn;            // ターン数
        public SkiiTarget Target;   // 使用対象
        public Param Param;         // パラメータ
        public float Coefficient;   // 係数
        public string Script;       // ロジックスクリプト
        public string ActionEffect; // 行動エフェクト
        public string ReceiveEffect;// 受けエフェクト
    }

    public sealed class SkillMap : ClassMap<Skill>
    {
        public SkillMap()
        {
            var index = 0;
            Map(x => x.Identify).Index(index++);
            Map(x => x.Name).Index(index++);
            Map(x => x.Cost).Index(index++);
            Map(x => x.Type).Index(index++);
            Map(x => x.Turn).Index(index++);
            Map(x => x.Target).Index(index++);
            Map(x => x.Param).Index(index++);
            Map(x => x.Coefficient).Index(index++);
            Map(x => x.Script).Index(index++);
            Map(x => x.ActionEffect).Index(index++);
            Map(x => x.ReceiveEffect).Index(index++);
    }
}
}
