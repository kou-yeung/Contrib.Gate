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

    public class PowerupWindow : Window, ANZListView.IDataSource
    {
        public const string CloseEvent = @"PowerupWindow:Close";
        public const string PowerupEvent = @"PowerupWindow:Powerup";

        public Text[] param;
        public ANZListView paramList;
        public GameObject prefab;
        public Text remainItem;
        public Text powerupCount;
        public Button powerupBtn;
        string uniqid;
        int hasItem; // 所持アイテム数

        int[] addParam = Enumerable.Repeat(0, (int)Score.Count).ToArray();

        Entities.PetItem Pet
        {
            get { return Entity.Instance.PetList.Find(uniqid); }
        }

        /// <summary>
        /// 残りパワーアップ回数
        /// </summary>
        int CanPowerupCount
        {
            get
            {
                return Mathf.Min(Pet.level * 4 - Pet.powerupCount, hasItem);
            }
        }

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;

            paramList.DataSource = this;
            Setup();

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
                        var type = EnumExtension<Score>.Parse(match.Groups[1].ToString());
                        var valueStr = match.Groups[2].ToString();
                        switch (valueStr)
                        {
                            case "+":
                                if(addParam.Sum() < CanPowerupCount) addParam[(int)type]++;
                                break;
                            case "-":
                                addParam[(int)type]--;
                                break;
                            default:
                                addParam[(int)type] = Mathf.Min(int.Parse(valueStr), CanPowerupCount - (addParam.Sum() - addParam[(int)type]));
                                break;
                        }
                        addParam[(int)type] = Mathf.Max(addParam[(int)type], 0);
                        Setup();
                    }
                    break;
            }
        }

        void Setup()
        {
            var pet = Pet;

            hasItem = Entity.Instance.Inventory.Count(new Identify(IDType.Material, 2001));
            remainItem.text = $"{hasItem - addParam.Sum()}";
            powerupCount.text = $"{ (pet.level * 4) - (pet.powerupCount + addParam.Sum())}";

            // 強化ボタンを有効にする
            powerupBtn.interactable = addParam.Sum() != 0;

            // パラメータ
            var baseParam = pet.Familiar.baseParam;
            var add = new List<int>();
            for (int i = 0; i < (int)Score.Count; i++)
            {
                add.Add(pet.param[i] + addParam[i]);
            }
            for (int i = 0; i < (int)Param.Count; i++)
            {
                var p = (Param)i;
                var before = pet.GetParam(p);
                var after = Entity.Instance.CalcParam(p, baseParam, add.ToArray());

                if (before != after)
                {
                    var diff = (before > after) ? "↓" : "↑";
                    param[i].text = $"{Entity.Instance.StringTable.Get(p)} : {after} ({diff} {Mathf.Abs(after - before)})";
                    param[i].color = before > after ? Color.blue : Color.red;
                }
                else
                {
                    param[i].text = $"{Entity.Instance.StringTable.Get(p)} : {after}";
                    param[i].color = Color.black;
                }
            }

            paramList.ReloadData();
        }


        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            Observer.Instance.Unsubscribe(PowerupItem.PowerupChangeEvent, OnSubscribe);
            base.OnClose();
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Powerup":
                    var send = new PowerupSend();
                    send.uniqid = uniqid;
                    send.param = addParam;
                    Protocol.Send(send, r =>
                    {
                        Entity.Instance.Inventory.Modify(r.items);
                        Entity.Instance.PetList.Modify(r.pet);
                        // 増加予定をクリアする
                        addParam = Enumerable.Repeat(0, addParam.Length).ToArray();
                        Setup();

                        Observer.Instance.Notify(PowerupEvent);
                    });
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }

        public int NumOfItems()
        {
            return (int)Score.Count;
        }

        float ANZListView.IDataSource.ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<PowerupItem>().Setup((Score)index, Pet, addParam[index]);
            return item;
        }
    }
}
