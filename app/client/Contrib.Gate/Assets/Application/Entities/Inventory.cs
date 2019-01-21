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

        public int Count(Identify identify)
        {
            var item = Find(identify);
            return (item != null) ? item.num : 0;
        }
        /// <summary>
        /// 増減
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="add"></param>
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

        /// <summary>
        /// 増減
        /// </summary>
        /// <param name="items"></param>
        public void Add(InventoryItem[] items)
        {
            foreach (var item in items)
            {
                Add(item.identify, item.num);
            }
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(InventoryItem item)
        {
            var index = items.FindIndex(i => i.identify == item.identify);
            if (index != -1) items[index] = item;
            else items.Add(item);

            // 0 以下のアイテムを非表示する
            items.RemoveAll(v => v.num <= 0);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(InventoryItem[] items)
        {
            foreach (var item in items)
            {
                Modify(item);
            }
        }
    }
}
