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
        public Slider timeGauge;
        public Text timeRemain;
        long current = 0;

        public void Setup(Entities.HatchItem item)
        {
            hatch = item;
            this.egg =  Entity.Instance.EggList.items.Find(v => v.uniqid == item.uniqid);
            var rarity = "";
            for (int i = 0; i < this.egg.rarity; i++) rarity += "★";
            this.rarity.text = rarity;
        }

        public void Update()
        {
            if (current == ServerTime.CurrentUnixTime) return;
            current = ServerTime.CurrentUnixTime;

            timeGauge.value = Mathf.Min(1, (current - hatch.startTime) / (float)hatch.timeRequired);

            var cur = UnixTime.FromUnixTime(current);
            var end = UnixTime.FromUnixTime(hatch.timeRequired + hatch.startTime);
            var remain = (end - cur);

            if (cur < end)
            {
                timeRemain.text = remain.ToString(@"hh\:mm\:ss");
            }
            else
            {
                timeRemain.text = "完了";
            }
        }
    }
}
