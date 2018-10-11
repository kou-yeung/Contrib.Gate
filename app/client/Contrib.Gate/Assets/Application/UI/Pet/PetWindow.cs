using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Event;
using System;
using Network;
using System.Linq;

namespace UI
{
    public class PetWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject petItemPrefab;
        public PetItem[] units;

        UnitList modify;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(petItemPrefab);
            item.GetComponent<PetItem>().Setup(Entity.Instance.PetList.items[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return petItemPrefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.PetList.items.Count;
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            // 詳細表示
            var pet = listItem.GetComponent<PetItem>().pet;
            Window.Open<PetDetailWindow>(pet.uniqid, modify);
            Observer.Instance.Subscribe(PetDetailWindow.ModifyEvent, OnSubscribe);
            Observer.Instance.Subscribe(PetDetailWindow.CloseEvent, OnSubscribe);
        }

        protected override void OnStart()
        {
            modify = Entity.Instance.UnitList.Clone();

            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.ReloadData();

            SetupUnit();
            Observer.Instance.Subscribe(UnitList.UpdateEvent, OnSubscribe);
            base.OnStart();
        }

        private void OnDestroy()
        {
            Observer.Instance.Unsubscribe(UnitList.UpdateEvent, OnSubscribe);
        }

        void OnSubscribe(string name, object o)
        {
            switch (name)
            {
                case UnitList.UpdateEvent:
                    SetupUnit();
                    break;
                case PetDetailWindow.CloseEvent:
                    Observer.Instance.Unsubscribe(PetDetailWindow.CloseEvent, OnSubscribe);
                    Observer.Instance.Unsubscribe(PetDetailWindow.ModifyEvent, OnSubscribe);
                    break;
                case PetDetailWindow.ModifyEvent:
                    var uniqid = o as string;

                    var index = Array.IndexOf(modify.items[0].uniqids, uniqid);
                    if (index != -1)
                    {
                        var count = modify.items[0].uniqids.Count(v => !string.IsNullOrEmpty(v));

                        if (count > 1)
                        {
                            // 外す
                            modify.items[0].uniqids[index] = "";

                            var v1 = modify.items[0].uniqids.Where(v => !string.IsNullOrEmpty(v));  // 空きではない
                            var v2 = modify.items[0].uniqids.Where(v => string.IsNullOrEmpty(v));   // 空き
                            modify.items[0].uniqids = v1.Concat(v2).ToArray();                      // 再連結 

                            modify.Modify(modify.items[0]);
                        }
                        else
                        {
                            // 1 体以下の場合、外せない!
                            DialogWindow.OpenOk("確認", "最低１体をセットしなさい!!");
                        }
                    }
                    else
                    {
                        var space = Array.IndexOf(modify.items[0].uniqids, "");
                        if (space == -1)
                        {
                            // 空きがない
                            DialogWindow.OpenOk("確認", "空き枠がありません");
                        } else
                        {
                            // セットする
                            modify.items[0].uniqids[space] = uniqid;
                            modify.Modify(modify.items[0]);
                        }
                    }
                    break;
            }
        }

        private void SetupUnit()
        {
            for (int i = 0; i < units.Length; i++)
            {
                var item = modify.items[0];
                for (int j = 0; j < item.uniqids.Length; j++)
                {
                    var pet = Entity.Instance.PetList.items.Find(v => v.uniqid == item.uniqids[j]);
                    if (pet != null)
                    {
                        units[j].Setup(pet);
                    }
                    units[j].gameObject.SetActive(!string.IsNullOrEmpty(item.uniqids[j]));
                }
            }
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "CloseButton":
                    if (Entity.Instance.UnitList.IsModify(modify))
                    {
                        Protocol.Send(new UnitUpdateSend { items = modify.items.ToArray() }, r =>
                        {
                            Entity.Instance.UnitList.Modify(r.items);
                            base.OnButtonClick(btn);
                        });
                    }
                    else
                    {
                        base.OnButtonClick(btn);
                    }
                    break;
            }
        }
    }
}
