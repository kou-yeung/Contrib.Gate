using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
    public class SkillSelectWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public GameObject prefab;
        public ANZCellView skills;

        string uniqid;
        List<Entities.InventoryItem> inventory = new List<Entities.InventoryItem>();
        Identify currentIdentify = Identify.Empty;

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;

            inventory.Clear();
            inventory.Add(new Entities.InventoryItem { identify = Identify.Empty, num = 0 });
            inventory.AddRange(Entity.Instance.Inventory.items.Where(v => new Identify(v.identify).Type == IDType.Skill));

            skills.DataSource = this;
            skills.ActionDelegate = this;
            skills.ReloadData();

            base.OnOpen(args);
        }
        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            bool selected = currentIdentify == inventory[index].identify;
            item.GetComponent<SkillSelectItem>().Setup(inventory[index], selected);
            return item;
        }

        public Vector2 ItemSize()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return inventory.Count();
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            currentIdentify = inventory[index].identify;
            skills.ReloadData();
        }
    }
}
