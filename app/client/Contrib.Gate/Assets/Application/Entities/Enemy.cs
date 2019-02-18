using CsvHelper.Configuration;

namespace Entities
{
    public class Enemy
    {
        public Identify Identify;
        public string Name;
        public Identify FamiliarID;
        public int[] Params;
    }

    public sealed class EnemyMap : ClassMap<Enemy>
    {
        public EnemyMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.FamiliarID).Index(2);
            Map(x => x.Params).Index(3, 3 + (int)Score.Count - 1);
        }
    }
}
