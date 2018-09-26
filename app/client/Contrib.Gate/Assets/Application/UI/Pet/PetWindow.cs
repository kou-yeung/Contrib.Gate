using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;

namespace UI
{
    public class PetWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject petItemPrefab;

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
            //Debug.Log("===================");
            //Debug.Log($"{pet.Familiar.Name} Lv({pet.Level})" );

            //for (int i = 0; i < (int)Param.Count; i++)
            //{
            //    Debug.Log($"{(Param)i} : {pet.GetParam((Param)i)}");
            //}
            //Debug.Log("===================");
        }

        protected override void OnStart()
        {
            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.ReloadData();
            base.OnStart();
        }
    }
}
