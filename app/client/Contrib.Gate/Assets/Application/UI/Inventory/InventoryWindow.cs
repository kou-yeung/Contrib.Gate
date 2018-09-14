using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;

namespace UI
{
    public class InventoryWindow : MonoBehaviour, ANZCellView.IDataSource
    {
        public GameObject prefab;
        public ANZCellView cell;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<InventoryItem>().Setup(Entity.Instance.Inventory.items[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Inventory.items.Count;
        }

        void Start()
        {
            cell.DataSource = this;
            cell.ReloadData();
        }
        public void OnClose()
        {
            Destroy(this.gameObject);
        }
    }
}

