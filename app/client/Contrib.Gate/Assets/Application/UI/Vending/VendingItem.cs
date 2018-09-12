using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using System.Linq;
using System;

namespace UI
{
    public class VendingItem : MonoBehaviour
    {
        public Image icon;
        public new Text name;
        public Text price;

        public void Setup(Vending data)
        {
            name.text = Entity.Instance.Name(data.Identify);
            price.text = $"{data.Num}個 ￥{data.Price}";
        }
    }
}
