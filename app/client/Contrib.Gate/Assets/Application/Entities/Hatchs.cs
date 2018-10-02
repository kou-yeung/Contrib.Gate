///==============================
/// 孵化データ一覧
///==============================
using System.Collections.Generic;

namespace Entities
{
    public class Hatchs
    {
        public List<HatchItem> items { get; private set; }
        public Hatchs(HatchItem[] items)
        {
            this.items = new List<HatchItem>(items);
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(HatchItem hatch)
        {
            var index = items.FindIndex(v => v.uniqid == hatch.uniqid);
            if (index != -1) items[index] = hatch;
            else items.Add(hatch);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(HatchItem[] hatchs)
        {
            if (hatchs == null) return;
            foreach (var hatch in hatchs)
            {
                Modify(hatch);
            }
        }

        /// <summary>
        /// 削除する
        /// </summary>
        /// <param name="egg"></param>
        public void Remove(HatchItem hatch)
        {
            Remove(hatch.uniqid);
        }

        public void Remove(string uniqid)
        {
            var index = items.FindIndex(v => v.uniqid == uniqid);
            if (index != -1) items.RemoveAt(index);
        }
    }
}

