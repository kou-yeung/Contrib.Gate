using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using Network;

namespace UI
{
    public class VendingWindow : Window, ANZListView.IDataSource, ANZListView.IActionDelegate, ANZListView.IPressDelegate
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
        protected override void OnStart()
        {
            list.DataSource = this;
            list.ActionDelegate = this;
            list.PressDelegate = this;
            list.ReloadData();
            base.OnStart();
        }

        public void TapListItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<VendingItem>();

            if (Entity.Instance.UserState.coin < item.vending.Price)
            {
                DialogWindow.OpenOk("確認", Entity.Instance.StringTable.Get(ErrorCode.CoinLack));
                return;
            }

            DialogWindow.OpenYesNo("確認", $"{item.vending.Price} コインを使って\n{Entity.Name(item.vending.Result)} x {item.vending.Num}個を交換しますか？",
                ()=>
                {
                    Protocol.Send(new VendingSend { identify = item.vending.Identify }, (r) =>
                    {
                        Debug.Log($"{Entity.Name(r.identify)} {r.current}個(+{r.added})");
                        Entity.Instance.Inventory.Add(r.identify, (int)r.added);
                        Entity.Instance.UserState.SetCoin(r.coin);
                    });
                });

        }

        public void PressListItem(int index, GameObject listItem)
        {
            Debug.Log($"PressListItem {index}");
        }
    }
}
