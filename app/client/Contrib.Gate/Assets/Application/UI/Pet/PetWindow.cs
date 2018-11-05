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
    public class PetWindow : Window, ANZCellView.IActionDelegate, ANZCellView.IPressDelegate//, ANZListView.IActionDelegate, ANZListView.IPressDelegate
    {
        public ANZCellView petCell;
        public GameObject petItemPrefab;

        public ANZCellView unitCell;
        public GameObject petInfoItemPrefab;

        UnitList modify;
        //============================
        // ペット一覧
        //============================
        class PetCell : ANZCellView.IDataSource
        {
            GameObject petItemPrefab;
            public PetCell(GameObject petItemPrefab)
            {
                this.petItemPrefab = petItemPrefab;
            }

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
        }
        //============================
        // ユニット
        //============================
        class UnitCell : ANZCellView.IDataSource
        {
            GameObject petInfoItemPrefab;
            public Func<UnitItem> GetCurrentUnit;

            public UnitCell(GameObject petInfoItemPrefab)
            {
                this.petInfoItemPrefab = petInfoItemPrefab;
            }
            public Vector2 ItemSize()
            {
                return petInfoItemPrefab.GetComponent<RectTransform>().sizeDelta;
            }

            public int NumOfItems()
            {
                return GetCurrentUnit().uniqids.Count(v => !string.IsNullOrEmpty(v));
            }

            public GameObject CellViewItem(int index, GameObject item)
            {
                if (item == null) item = Instantiate(petInfoItemPrefab);
                var unit = GetCurrentUnit();
                var pet = Entity.Instance.PetList.items.Find(v => v.uniqid == unit.uniqids[index]);
                item.GetComponent<PetInfo>().Setup(pet);
                return item;
            }
        }
        //public void TapListItem(int index, GameObject listItem)
        //{
        //    var item = listItem.GetComponent<PetInfo>();
        //    Modify(item.pet.uniqid);
        //}
        //public void PressListItem(int index, GameObject listItem)
        //{
        //    // 詳細表示
        //    var pet = listItem.GetComponent<PetInfo>().pet;
        //    Window.Open<PetDetailWindow>(pet.uniqid);
        //    Observer.Instance.Subscribe(PetDetailWindow.CloseEvent, OnSubscribe);
        //}

        //============================
        // Window自身
        //============================

        public void TapCellItem(int index, GameObject listItem)
        {
            string uniq = "";

            var item = listItem.GetComponent<PetItem>();
            if (item != null) uniq = item.pet.uniqid;

            var info = listItem.GetComponent<PetInfo>();
            if (info != null) uniq = info.pet.uniqid;

            if (!string.IsNullOrEmpty(uniq)) Modify(uniq);
        }
        public void PressCellItem(int index, GameObject listItem)
        {
            // 詳細表示
            string uniq = "";

            var item = listItem.GetComponent<PetItem>();
            if (item != null) uniq = item.pet.uniqid;

            var info = listItem.GetComponent<PetInfo>();
            if (info != null) uniq = info.pet.uniqid;

            if (!string.IsNullOrEmpty(uniq))
            {
                Open<PetDetailWindow>(uniq);
                Observer.Instance.Subscribe(PetDetailWindow.CloseEvent, OnSubscribe);
            }
        }

        protected override void OnStart()
        {
            modify = Entity.Instance.UnitList.Clone();

            petCell.DataSource = new PetCell(petItemPrefab);
            petCell.ActionDelegate = this;
            petCell.PressDelegate = this;

            var view = new UnitCell(petInfoItemPrefab);
            unitCell.DataSource = view;
            unitCell.ActionDelegate = this;
            unitCell.PressDelegate = this;
            view.GetCurrentUnit = () => { return modify.items[0]; };

            SetupUnit();
            Observer.Instance.Subscribe(UnitList.UpdateEvent, OnSubscribe);
            base.OnStart();
        }

        private void OnDestroy()
        {
            Observer.Instance.Unsubscribe(UnitList.UpdateEvent, OnSubscribe);
        }

        /// <summary>
        /// 指定IDが編集された
        /// </summary>
        /// <param name="uniqid"></param>
        void Modify(string uniqid)
        {
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
                }
                else
                {
                    // セットする
                    modify.items[0].uniqids[space] = uniqid;
                    modify.Modify(modify.items[0]);
                }
            }
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
                    SetupUnit();
                    break;
            }
        }

        private void SetupUnit()
        {
            petCell.ReloadData();
            unitCell.ReloadData();
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
