﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Entities;

namespace UI
{
    public class EggItem : MonoBehaviour
    {
        public Entities.EggItem egg { get; private set; }
        public Text rarity;
        public GameObject hatch;

        public void Setup(Entities.EggItem item)
        {
            this.egg = item;
            var rarity = "";
            for (int i = 0; i < this.egg.rarity; i++)
            {
                rarity += "★";
            }
            this.rarity.text = rarity;

            // 予約中 の場合[予約中]ラベルを表示する
            var data = Entity.Instance.Hatchs.items.Find(v => v.uniqid == item.uniqid);
            hatch.SetActive(data != null);
        }
    }
}
