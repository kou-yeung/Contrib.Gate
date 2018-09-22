using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EggItem : MonoBehaviour
    {
        public Entities.EggItem egg { get; private set; }
        public Text rarity;

        public void Setup(Entities.EggItem item)
        {
            this.egg = item;
            var rarity = "";
            for (int i = 0; i < this.egg.rarity; i++)
            {
                rarity += "★";
            }
            this.rarity.text = rarity;
        }
    }
}
