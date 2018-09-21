///==============================
/// 未孵化のタマゴ一覧
///==============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Eggs
    {
        public List<EggItem> items { get; private set; }
        public Eggs(EggItem[] items)
        {
            this.items = new List<EggItem>(items);
        }
    }
}

