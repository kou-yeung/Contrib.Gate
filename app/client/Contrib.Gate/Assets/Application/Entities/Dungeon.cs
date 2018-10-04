using CsvHelper.Configuration;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class Dungeon
    {
        public Identify Identify;
        public string Name;
        public Identify UpFloor;
        public Identify DownFloor;
        public Identify Room;
        public string AssetPath;
        public int EncountRate;
    }

    public sealed class DungeonMap : ClassMap<Dungeon>
    {
        public DungeonMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.UpFloor).Index(2);
            Map(x => x.DownFloor).Index(3);
            Map(x => x.Room).Index(4);
            Map(x => x.AssetPath).Index(5);
            Map(x => x.EncountRate).Index(6);
        }
    }

}
