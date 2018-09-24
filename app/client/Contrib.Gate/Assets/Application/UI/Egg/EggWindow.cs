using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;

namespace UI
{
    public class EggWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject eggItemprefab;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(eggItemprefab);
            item.GetComponent<EggItem>().Setup(Entity.Instance.Eggs.items[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return eggItemprefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Eggs.items.Count;
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            var egg = listItem.GetComponent<EggItem>().egg;
            DialogWindow.OpenYesNo("確認", $"{egg.race}のタマゴを孵化予約します", () =>
            {
                Protocol.Send(new HatchReserveSend { uniqid = egg.uniqid }, (r) =>
                {
                    Entity.Instance.Hatchs.Modify(r.item);
                    cell.ReloadData();  // リスト更新
                });
            });
        }

        protected override void OnStart()
        {
            cell.DataSource = this;
            cell.ActionDelegate = this;
            cell.ReloadData();
            base.OnStart();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
