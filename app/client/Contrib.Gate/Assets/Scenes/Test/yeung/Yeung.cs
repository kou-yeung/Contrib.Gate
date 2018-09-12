using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;

namespace Test
{
    public class Yeung : MonoBehaviour, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public GameObject prefab;
        public ANZListView list;

        public float HeightItem()
        {
            return prefab.GetComponent<RectTransform>().rect.height;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);

            item.GetComponent<Image>().color = new Color(index / 255f, index / 255f, index / 255f, 1.0f);
            return item;
        }

        public int NumOfItems()
        {
            return 255;
        }

        public void TapListItem(int index, GameObject listItem)
        {
            Debug.Log(index);
        }

        // Use this for initialization
        void Start()
        {
            list.DataSource = this;
            list.ActionDelegate = this;
            list.ReloadData();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
