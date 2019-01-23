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
using System.Text.RegularExpressions;
using Util;

namespace UI
{

    public class PowerupWindow : Window, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public const string CloseEvent = @"PowerupWindow:Close";
        public const string PowerupEvent = @"PowerupWindow:Powerup";

        //public Text[] afterParam;
        public ANZListView paramList;
        public GameObject prefab;
        public Text remainItem;
        public Text powerupCount;
        string uniqid;
        //int remain; // 残り強化回数

        //List<Entities.InventoryItem> hasItems = new List<Entities.InventoryItem>();  // 所持アイテム
        //List<Entities.InventoryItem> useItems = new List<Entities.InventoryItem>();  // 使用予定アイテム一覧

        static readonly Param[] showParam = { Param.HP, Param.MP, Param.PhysicalAttack, Param.PhysicalDefense, Param.MagicAttack, Param.MagicDefense, Param.Agility };

        /// <summary>
        /// 使用予定アイテム一覧から、必要パワーアップ回数を計算する
        /// </summary>
        int needPowerCount
        {
            get
            {
                var res = 0;
                //foreach (var item in useItems)
                //{
                //    var info = Array.Find(Entity.Instance.Items, v => v.Identify == item.identify);
                //    res += info.PowerupCost * item.num;
                //}
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
            //foreach (var item in useItems)
            //{
            //    var info = Array.Find(Entity.Instance.Items, v => v.Identify == item.identify);
            //    res += info.Effects.Where(v => v.param == param).Sum(v => v.value * item.num);
            //}
            return res;
        }

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;
            Setup();

            paramList.DataSource = this;
            paramList.ActionDelegate = this;
            UpdateCell();


            Observer.Instance.Subscribe(PowerupItem.PowerupChangeEvent, OnSubscribe);

            base.OnOpen(args);
        }

        void OnSubscribe(string name, object obj)
        {
            switch (name)
            {
                case PowerupItem.PowerupChangeEvent:
                    var match = Regex.Match(obj as string, @"(.*):(.*)");
                    if (match != Match.Empty)
                    {
                        var type = EnumExtension<Param>.Parse(match.Groups[1].ToString());
                        var valueStr = match.Groups[2].ToString();
                        switch (valueStr)
                        {
                            case "+":
                                Debug.Log($"{type} +");
                                break;
                            case "-":
                                Debug.Log($"{type} -");
                                break;
                            default:
                                var value = uint.Parse(valueStr);
                                Debug.Log($"{type} {value}");
                                break;
                        }
                    }
                    break;
            }
        }

        void Setup()
        {
            var pet = Entity.Instance.PetList.Find(uniqid);
            remainItem.text = "999";// ((pet.level - pet.powerupCount)- needPowerCount).ToString();
            powerupCount.text = $"{pet.powerupCount}/{pet.level}";

            //// パラメータ設定
            //for (int i = 0; i < (int)Param.Count; i++)
            //{
            //    if ((Param)i == Param.Luck) continue;
            //    var value = pet.GetParam((Param)i);
            //    var grow = value + Grow((Param)i);
            //    afterParam[i].text = grow.ToString();

            //    afterParam[i].color = (value != grow) ? Color.red : Color.white;
            //}
        }

        void UpdateCell()
        {
            //// 所持アイテム一覧作成
            //hasItems = Entity.Instance.Inventory.items.Where(v => new Identify(v.identify).Type == IDType.Item).ToList();
            paramList.ReloadData();
            Setup();
        }
        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            Observer.Instance.Unsubscribe(PowerupItem.PowerupChangeEvent, OnSubscribe);
            base.OnClose();
        }

        protected override void OnButtonClick(Button btn)
        {
            //switch (btn.name)
            //{
            //    case "Powerup":
            //        var send = new PowerupSend();
            //        send.uniqid = uniqid;
            //        send.items = useItems.ToArray();
            //        Protocol.Send(send, r =>
            //        {
            //            Entity.Instance.Inventory.Modify(r.items);
            //            Entity.Instance.PetList.Modify(r.pet);
            //            useItems.Clear();
            //            UpdateCell();

            //            Observer.Instance.Notify(PowerupEvent);
            //        });
            //        break;
            //}
            base.OnButtonClick(btn);
        }

        public int NumOfItems()
        {
            return showParam.Length;
        }

        public void TapListItem(int index, GameObject listItem)
        {
            //var pet = Entity.Instance.PetList.Find(uniqid);
            //var remain = (pet.level - pet.powerupCount);
            //if (needPowerCount >= remain) return;
            //if (hasItems[index].num <= 0) return;
            //hasItems[index].num -= 1;
            //var item = useItems.Find(v => v.identify == hasItems[index].identify);
            //if (item != null) item.num++;
            //else useItems.Add(new Entities.InventoryItem { identify = hasItems[index].identify, num = 1 });
            //UpdateCell();
        }

        float ANZListView.IDataSource.ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            var param = showParam[index];
            item.GetComponent<PowerupItem>().Setup(param);
            return item;
        }
    }
}
