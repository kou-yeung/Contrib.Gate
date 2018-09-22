﻿///==============================
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

        /// <summary>
        /// 変更する
        /// </summary>
        /// <param name="egg"></param>
        public void Modify(EggItem egg)
        {
            var index = items.FindIndex(v => v.uniqid == egg.uniqid);
            if (index != -1) items[index] = egg;
            else items.Add(egg);
        }
    }
}

