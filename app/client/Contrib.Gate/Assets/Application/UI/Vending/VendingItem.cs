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
        public Vending vending { get; private set; }

        public void Setup(Vending data)
        {
            vending = data;
            name.text = Entity.Name(data.Result);
            price.text = $"{data.Num}個 ￥{data.Price}";
        }
    }
}
