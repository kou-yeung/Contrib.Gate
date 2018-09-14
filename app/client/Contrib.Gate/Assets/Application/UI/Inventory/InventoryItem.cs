using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryItem : MonoBehaviour
    {
        public Entities.InventoryItem item;
        public new Text name;
        public Text num;

        public void Setup(Entities.InventoryItem item)
        {
            this.item = item;
            name.text = Entities.Entity.Name(item.identify);
            num.text = $"ｘ{item.num}";
        }
    }
}
