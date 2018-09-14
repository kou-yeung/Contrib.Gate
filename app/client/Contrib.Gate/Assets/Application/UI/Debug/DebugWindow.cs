﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using System.Linq;

namespace UI
{
    public class DebugWindow : MonoBehaviour, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public new Text name;
        public DebugParam[] Params;
        public ANZListView list;
        public GameObject prefab;
        Cheat current;

        public float HeightItem()
        {
            return prefab.GetComponent<RectTransform>().sizeDelta.y;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(prefab);
            item.GetComponent<DebugItem>().Setup(Entity.Instance.Cheats[index]);
            return item;
        }

        public int NumOfItems()
        {
            return Entity.Instance.Cheats.Length;
        }

        public void TapListItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<DebugItem>();
            var cheat = item.cheat;
            name.text = cheat.Name;

            for (int i = 0; i < Params.Length; i++)
            {
                if (i < cheat.Params.Count)
                {
                    Params[i].gameObject.SetActive(true);
                    Params[i].name.text = cheat.Params[i];
                }
                else
                {
                    Params[i].gameObject.SetActive(false);
                }
            }
            current = item.cheat;
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

        public void OnClickExec()
        {
            if (current == null) return;
            var send = new CheatSend();
            send.command = current.Command;
            send.param = Params.Where(p => p.gameObject.activeSelf).Select(p => p.input.text).ToArray();
            Protocol.Send(send, (r) =>
            {
                switch (send.command)
                {
                    case "addcoin":
                        Entity.Instance.UpdateUserState(r.userState);
                        break;
                }
                //Debug.Log(r.message);
            });   
        }
    }
}
