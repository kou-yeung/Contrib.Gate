///==============================
/// ステージ一覧
///==============================
using System.Collections.Generic;

namespace Entities
{
    public class StageList
    {
        public List<StageItem> items { get; private set; }
        public StageList(StageItem[] items)
        {
            this.items = new List<StageItem>(items);
        }

        /// <summary>
        /// 変更する
        /// </summary>
        public void Modify(StageItem item)
        {
            var index = items.FindIndex(v => v.id == item.id);
            if (index != -1) items[index] = item;
            else items.Add(item);
        }
        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(StageItem[] items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                Modify(item);
            }
        }

    }
}
