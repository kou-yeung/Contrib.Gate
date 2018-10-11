using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Inventory
    {
        public List<InventoryItem> items { get; private set; }

        public Inventory(InventoryItem[] items)
        {
            this.items = new List<InventoryItem>(items);
        }

        public InventoryItem Find(Identify identify)
        {
            return items.Find(item => item.identify == identify);
        }

        public void Add(Identify identify, int add)
        {
            var item = items.Find(i => i.identify == identify);
            if (item != null)
            {
                item.num += add;
            } else
            {
                items.Add(new InventoryItem { identify = identify, num = add });
            }
            // 0 以下のアイテムを非表示する
            items.RemoveAll(v => v.num <= 0);
        }

        public void Add(InventoryItem[] items)
        {
            foreach (var item in items)
            {
                Add(item.identify, item.num);
            }
        }
    }
}
