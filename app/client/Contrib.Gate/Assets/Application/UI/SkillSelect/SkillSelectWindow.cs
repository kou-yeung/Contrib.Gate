using UnityEngine;
using Xyz.AnzFactory.UI;
using Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Network;
using EventSystem;

namespace UI
{
    public class SkillSelectWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public const string CloseEvent = @"SkillSelectWindow:Close";
        public const string ChangeEvent = @"SkillSelectWindow:Change";
        
        public GameObject prefab;
        public ANZCellView skills;
        public Button selectBtn;

        string uniqid;
        List<Entities.InventoryItem> inventory = new List<Entities.InventoryItem>();
        int currentIndex = -1;

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;

            inventory.Clear();
            inventory.Add(new Entities.InventoryItem { identify = Identify.Empty, num = 0 });
            inventory.AddRange(Entity.Instance.Inventory.items.Where(v => new Identify(v.identify).Type == IDType.Skill));

            skills.DataSource = this;
            skills.ActionDelegate = this;
            skills.ReloadData();

            selectBtn.interactable = false;

            base.OnOpen(args);
        }
        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<SkillSelectItem>().Setup(inventory[index], currentIndex == index);
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
            currentIndex = index;
            selectBtn.interactable = true;
            skills.ReloadData();
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }
        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Select":
                    var send = new SkillLearnSend();
                    send.uniqid = uniqid;
                    send.skill = inventory[currentIndex].identify;
                    Protocol.Send(send, r =>
                    {
                        Entity.Instance.Inventory.Modify(r.items);
                        Entity.Instance.PetList.Modify(r.pet);

                        Observer.Instance.Notify(ChangeEvent);
                        this.Close();
                    });
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }
    }
}
