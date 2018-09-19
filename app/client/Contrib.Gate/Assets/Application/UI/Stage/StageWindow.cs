using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;

namespace UI
{
    public class StageWindow : Window, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public ANZListView list;
        public GameObject stageItemPrefab;

        public float HeightItem()
        {
            return stageItemPrefab.GetComponent<RectTransform>().rect.height;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(stageItemPrefab);
            item.GetComponent<StageItem>().Setup(Entity.Instance.Stages[index]);
            return item;
        }

        public int NumOfItems()
        {
            /// MEMO : とりあえずすべてステージを表示する
            return Entity.Instance.Stages.Length;
        }

        public void TapListItem(int index, GameObject listItem)
        {
        }

        protected override void OnStart()
        {
            list.DataSource = this;
            list.ActionDelegate = this;
            list.ReloadData();
            base.OnStart();
        }

    }
}
