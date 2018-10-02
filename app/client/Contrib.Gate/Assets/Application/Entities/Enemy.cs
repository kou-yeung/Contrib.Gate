﻿using CsvHelper.Configuration;

namespace Entities
{
    public class Enemy
    {
        public Identify Identify;
        public string Name;
        public Race Race;
        public int Rarity;
        public int[] baseParam;     // レベル1時のパラメータ
        public int[] additionParam; // レベル1上昇時のパラメータ増加
    }

    public sealed class EnemyMap : ClassMap<Enemy>
    {
        public EnemyMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Race).Index(2);
            Map(x => x.Rarity).Index(3);
            Map(x => x.baseParam).Index(4, 4 + (int)Param.Count - 1);
            Map(x => x.additionParam).Index(12, 12 + (int)Param.Count - 1);
        }
    }
}