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

        public ANZListView paramList;
        public GameObject prefab;
        public Text remainItem;
        public Text powerupCount;
        public Button powerupBtn;
        string uniqid;
        int hasItem; // 所持アイテム数

        int[] addParam = Enumerable.Repeat(0, (int)Param.Count).ToArray();
        static readonly Param[] showParam = { Param.HP, Param.MP, Param.PhysicalAttack, Param.PhysicalDefense, Param.MagicAttack, Param.MagicDefense, Param.Agility };

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
                return Mathf.Min(Pet.level - Pet.powerupCount, hasItem);
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
                        var type = EnumExtension<Param>.Parse(match.Groups[1].ToString());
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
                        addParam[(int)type] = Mathf.Clamp(addParam[(int)type], 0, 99);
                        Setup();
                    }
                    break;
            }
        }

        void Setup()
        {
            hasItem = Entity.Instance.Inventory.Count(new Identify(IDType.Material, 2001));
            remainItem.text = $"{hasItem - addParam.Sum()}";
            powerupCount.text = $"{Pet.powerupCount + addParam.Sum()}/{Pet.level}";

            // 強化ボタンを有効にする
            powerupBtn.interactable = addParam.Sum() != 0;

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
            return showParam.Length;
        }

        float ANZListView.IDataSource.ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            var param = showParam[index];
            item.GetComponent<PowerupItem>().Setup(param, Pet, addParam[(int)param]);
            return item;
        }
    }
}
