using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class PetItem : MonoBehaviour
    {
        public Entities.PetItem pet { get; private set; }
        public Image icon;
        public Text rarity;
        public void Setup(Entities.PetItem item)
        {
            this.pet = item;
            var rarity = "";
            var id = new Entities.Identify(item.id);
            icon.sprite = Resources.Load<Sprite>($"test/familiar/{id.Id}");

            var familiar = Array.Find(Entities.Entity.Instance.Familiars, v => v.Identify == id);
            if (familiar != null)
            {
                for (int i = 0; i < familiar.Rarity; i++)
                {
                    rarity += "★";
                }
                this.rarity.text = rarity;
            }
        }
    }
}
