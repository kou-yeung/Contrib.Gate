using CsvHelper.Configuration;
using System.Collections.Generic;
using Util;
using UnityEngine;

namespace Entities
{
    public class Item
    {
        public class Effect
        {
            public Param param;    // 対象パラメータ
            public int value;      // 影響量
        }

        public Identify Identify;
        public string Name;             // 名前
        public int Rarity;              // レアリティ
        public string Image;            // 画像ID
        public int PowerupCost;         // 餌増加
        public List<Effect> Effects;    // 効果
        public string Desc;             // 説明文
    }

    public sealed class ItemMap : ClassMap<Item>
    {
        public ItemMap()
        {
            Map(x => x.Identify).Index(0);
            Map(x => x.Name).Index(1);
            Map(x => x.Rarity).Index(2);
            Map(x => x.Image).Index(3);
            Map(x => x.PowerupCost).Index(4);
            Map(x => x.Effects).ConvertUsing(row =>
            {
                var res = new List<Item.Effect>();
                for (int i = 0; i < 4; i += 2)
                {
                    var offset = 5 + i;
                    string param; int value;
                    if (!row.TryGetField(offset++, out param)) continue;
                    if (!row.TryGetField(offset++, out value)) continue;
                    res.Add(new Item.Effect { param = EnumExtension<Param>.Parse(param), value = value });
                }
                return res;
            });
            Map(x => x.Desc).Index(9);
        }
    }
}
