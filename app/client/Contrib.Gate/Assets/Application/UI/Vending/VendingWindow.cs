using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using Network;

namespace UI
{
    public class VendingWindow : MonoBehaviour, ANZListView.IDataSource, ANZListView.IActionDelegate
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
            list.ActionDelegate = this;
            list.ReloadData();
        }

        public void OnClose()
        {
            Destroy(this.gameObject);
        }

        public void TapListItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<VendingItem>();
            Protocol.Send(new VendingSend { identify = item.vending.Identify }, (r) =>
            {
                Debug.Log($"{Entity.Name(r.identify)} {r.current}個(+{r.added})");
                Entity.Instance.inventory.Add(r.identify, (int)r.added);
            });
        }
    }
}
