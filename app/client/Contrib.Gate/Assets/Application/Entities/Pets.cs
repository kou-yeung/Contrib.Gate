///==============================
/// ペット一覧
///==============================
using System.Collections.Generic;
using Network;

namespace Entities
{
    public class Pets
    {
        public List<PetItem> items { get; private set; }
        public Pets(PetItem[] items)
        {
            this.items = new List<PetItem>(items);
        }

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(PetItem pet)
        {
            var index = items.FindIndex(v => v.uniqid == pet.uniqid);
            if (index != -1) items[index] = pet;
            else items.Add(pet);
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
            }
        }
    }
}

