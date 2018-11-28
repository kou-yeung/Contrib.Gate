using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using EventSystem;
using UnityEngine.UI;
using System.Linq;
using System;
using Network;

namespace UI
{

    public class PowerupWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public const string CloseEvent = @"PowerupWindow:Close";
        public const string PowerupEvent = @"PowerupWindow:Powerup";

        public Text[] afterParam;
        public ANZCellView inventory;
        public GameObject prefab;
        public Text remain;
        string uniqid;
        //int remain; // 残り強化回数

        List<Entities.InventoryItem> hasItems = new List<Entities.InventoryItem>();  // 所持アイテム
        List<Entities.InventoryItem> useItems = new List<Entities.InventoryItem>();  // 使用予定アイテム一覧

        /// <summary>
        /// 使用予定アイテム一覧から、必要パワーアップ回数を計算する
        /// </summary>
        int needPowerCount
        {
            get
            {
                var res = 0;
                foreach (var item in useItems)
                {
                    var info = Array.Find(Entity.Instance.Items, v => v.Identify == item.identify);
                    res += info.PowerupCost * item.num;
                }
                return res;
            }
        }

        /// <summary>
        /// 指定パラメータの増加予定量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        int Grow(Param param)
        {
            var res = 0;
            foreach (var item in useItems)
            {
                var info = Array.Find(Entity.Instance.Items, v => v.Identify == item.identify);
                res += info.Effects.Where(v => v.param == param).Sum(v => v.value * item.num);
            }
            return res;
        }

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;
            Setup();

            inventory.DataSource = this;
            inventory.ActionDelegate = this;
            UpdateCell();

            base.OnOpen(args);
        }

        void Setup()
        {
            var pet = Entity.Instance.PetList.Find(uniqid);
            remain.text = ((pet.level - pet.powerupCount)- needPowerCount).ToString();

            // パラメータ設定
            for (int i = 0; i < (int)Param.Count; i++)
            {
                if ((Param)i == Param.Luck) continue;
                var value = pet.GetParam((Param)i);
                var grow = value + Grow((Param)i);
                afterParam[i].text = grow.ToString();

                afterParam[i].color = (value != grow) ? Color.red : Color.white;
            }
        }

        void UpdateCell()
        {
            // 所持アイテム一覧作成
            hasItems = Entity.Instance.Inventory.items.Where(v => new Identify(v.identify).Type == IDType.Item).ToList();
            inventory.ReloadData();
            Setup();
        }
        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Powerup":
                    var send = new PowerupSend();
                    send.uniqid = uniqid;
                    send.items = useItems.ToArray();
                    Protocol.Send(send, r =>
                    {
                        Entity.Instance.Inventory.Modify(r.items);
                        Entity.Instance.PetList.Modify(r.pet);
                        useItems.Clear();
                        UpdateCell();

                        Observer.Instance.Notify(PowerupEvent);
                    });
                    break;
            }
            base.OnButtonClick(btn);
        }

        public int NumOfItems()
        {
            return hasItems.Count;
        }

        public Vector2 ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta;
        }

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<InventoryItem>().Setup(hasItems[index]);
            return item;
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            var pet = Entity.Instance.PetList.Find(uniqid);
            var remain = (pet.level - pet.powerupCount);

            if (needPowerCount >= remain) return;
            if (hasItems[index].num <= 0) return;

            hasItems[index].num -= 1;
            var item = useItems.Find(v => v.identify == hasItems[index].identify);
            if (item != null) item.num++;
            else useItems.Add(new Entities.InventoryItem { identify = hasItems[index].identify, num = 1 });
            UpdateCell();
        }
    }
}
