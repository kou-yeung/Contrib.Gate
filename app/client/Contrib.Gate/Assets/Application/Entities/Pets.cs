///==============================
/// ペット一覧
///==============================
using System.Collections.Generic;

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

    }
}

