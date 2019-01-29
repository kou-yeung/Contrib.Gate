using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

namespace UI
{
    public class SkillSelectItem : MonoBehaviour
    {
        public Text desc;
        public Entities.InventoryItem item { get; private set; }

        public void Setup(Entities.InventoryItem item, bool selected)
        {
            this.item = item;

            if (item.identify == Identify.Empty)
            {
                desc.text = "はずす";
            }
            else
            {
                desc.text = $"{Entity.Name(item.identify)}\n{item.num}";
            }

            GetComponentInChildren<Image>().color = selected ? Color.red : Color.white;
        }
    }
}
