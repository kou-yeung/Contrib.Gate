using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Event;

namespace UI
{
    public class PetWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject petItemPrefab;
        public PetItem[] units;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(petItemPrefab);
            item.GetComponent<PetItem>().Setup(Entity.Instance.Pets.items[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return petItemPrefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Pets.items.Count;
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            // 詳細表示
            var pet = listItem.GetComponent<PetItem>().pet;
            Window.Open<PetDetailWindow>(pet.uniqid);
        }

        protected override void OnStart()
        {
            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.ReloadData();

            SetupUnit();
            Observer.Instance.Subscribe(Units.UpdateEvent, OnSubscribe);

            base.OnStart();
        }

        private void OnDestroy()
        {
            Observer.Instance.Unsubscribe(Units.UpdateEvent, OnSubscribe);
        }

        void OnSubscribe(string name, object o)
        {
            switch (name)
            {
                case Units.UpdateEvent:
                    SetupUnit();
                    break;
            }
        }

        private void SetupUnit()
        {
            for (int i = 0; i < units.Length; i++)
            {
                var item = Entity.Instance.Units.items[0];
                for (int j = 0; j < item.uniqids.Length; j++)
                {
                    var pet = Entity.Instance.Pets.items.Find(v => v.uniqid == item.uniqids[j]);
                    if (pet != null)
                    {
                        units[j].Setup(pet);
                    }
                    units[j].gameObject.SetActive(!string.IsNullOrEmpty(item.uniqids[j]));
                }
            }
        }
    }
}
