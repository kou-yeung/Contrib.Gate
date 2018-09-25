using CsvHelper.Configuration;
using System.Collections.Generic;
using System;
namespace Entities
{
    public class Level
    {
        public int level;   // レベル
        public int exp;     // 必須経験値
    }

    public class LevelTable
    {
        List<Level> items;
        public LevelTable(Level[] items)
        {
            this.items = new List<Level>(items);
        }

        /// <summary>
        /// exp -> level を計算する
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int Level(int exp)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (exp < items[i].exp) return i;
            }
            return items.Count;
        }
        /// <summary>
        /// level -> exp を計算する
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int Exp(int level)
        {
            var index = Math.Min(items.Count - 1, level - 1);
            return items[index].exp;
        }
    }

    public sealed class LevelMap : ClassMap<Level>
    {
        public LevelMap()
        {
            Map(x => x.level).Index(0);
            Map(x => x.exp).Index(1);
        }
    }
}
