using CsvHelper.Configuration;

namespace Entities
{
    public class Stage
    {
        public Identify Identify;
        public string Name;
        public Identify Dungeon;    // 開始ダンジョンフロアID
    }

    public sealed class StageMap : ClassMap<Stage>
    {
        public StageMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Dungeon).Index(2);
        }
    }
}
