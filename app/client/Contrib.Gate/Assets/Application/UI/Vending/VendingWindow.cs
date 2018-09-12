using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;

namespace UI
{
    public class VendingWindow : MonoBehaviour, ANZListView.IDataSource
    {
        public ANZListView list;
        public GameObject vendingItemPrefab;

        public float HeightItem()
        {
            return vendingItemPrefab.GetComponent<RectTransform>().rect.height;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(vendingItemPrefab);
            item.GetComponent<VendingItem>().Setup(Entity.Instance.Vendings[index]);
            return item;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Vendings.Length;
        }

        // Use this for initialization
        void Start()
        {
            list.DataSource = this;
            list.ReloadData();
        }
    }
}
