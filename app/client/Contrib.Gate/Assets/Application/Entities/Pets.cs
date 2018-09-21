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
    }
}

