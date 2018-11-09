using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Util.Time;

namespace UI
{
    public class HatchItem : MonoBehaviour
    {
        public Entities.HatchItem hatch { get; private set; }
        public Entities.EggItem egg { get; private set; }
        public Text rarity;
        public GameObject hatchBG;
        public Text hatchText;

        public void Setup(Entities.HatchItem item)
        {
            hatch = item;
            this.egg =  Entity.Instance.EggList.items.Find(v => v.uniqid == item.uniqid);
            if (this.egg.judgment)
            {
                var rarity = "";
                for (int i = 0; i < this.egg.rarity; i++) rarity += "★";
                this.rarity.text = rarity;
            }
            else
            {
                this.rarity.text = "？";
            }

            // 予約中 の場合[予約中]ラベルを表示する
            var data = Entity.Instance.HatchList.items.Find(v => v.uniqid == item.uniqid);
            hatchBG.SetActive(data != null);

            if (data != null)
            {
                var endtime = data.startTime + data.timeRequired;
                hatchText.text = (endtime < ServerTime.CurrentUnixTime) ? "孵化完了" : "孵化中";
            }
        }
    }
}
