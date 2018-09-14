using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;

namespace UI
{
    public class DebugWindow : MonoBehaviour, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public new Text name;
        public DebugParam[] Params;
        public ANZListView list;
        public GameObject prefab;

        public float HeightItem()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<DebugItem>().Setup(Entity.Instance.cheats[index]);
            return item;
        }

        public int NumOfItems()
        {
            return Entity.Instance.cheats.Length;
        }

        public void TapListItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<DebugItem>();
            var cheat = item.cheat;
            name.text = cheat.Name;
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
    }
}
