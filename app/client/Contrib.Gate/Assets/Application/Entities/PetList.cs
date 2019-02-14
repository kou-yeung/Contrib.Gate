///==============================
/// ペット一覧
///==============================
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class PetList
    {
        public List<PetItem> items { get; private set; }
        public PetList(PetItem[] items)
        {
            this.items = new List<PetItem>(items);
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="pet"></param>
        public void Modify(PetItem pet)
        {
            var index = items.FindIndex(v => v.uniqid == pet.uniqid);
            if (index != -1) items[index] = pet;
            else items.Add(pet);
        }

        /// <summary>
        /// 削除する
        /// </summary>
        /// <param name="pet"></param>
        public void Remove(PetItem pet)
        {
            var index = items.FindIndex(v => v.uniqid == pet.uniqid);
            if (index != -1) items.RemoveAt(index);
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(PetItem[] pets)
        {
            if (pets == null) return;
            foreach (var pet in pets)
            {
                Modify(pet);
            }
        }

        /// <summary>
        /// 経験値を更新する
        /// </summary>
        public void Modify(ExpItem[] items)
        {
            foreach (var item in items)
            {
                var index = this.items.FindIndex(v => v.uniqid == item.uniqid);
                if (index == -1) continue;
                this.items[index].exp = item.exp;
                this.items[index].level = item.level;
            }
        }

        /// <summary>
        /// uniqid から検索します
        /// </summary>
        /// <param name="uniqid"></param>
        /// <returns></returns>
        public PetItem Find(string uniqid)
        {
            var index = this.items.FindIndex(v => v.uniqid == uniqid);
            if (index == -1) return null;
            return this.items[index];
        }
    }
}

