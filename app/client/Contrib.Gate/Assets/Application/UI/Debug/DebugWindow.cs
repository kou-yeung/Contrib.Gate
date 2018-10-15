using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using System.Linq;

namespace UI
{
    public class DebugWindow : Window, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public GameObject Background;
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
                    Params[i].name.text = cheat.Params[i].name;
                    Params[i].input.text = cheat.Params[i].defaultValue;
                }
                else
                {
                    Params[i].gameObject.SetActive(false);
                }
            }
            current = item.cheat;
        }

        // Use this for initialization
        protected override void OnStart()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            list.DataSource = this;
            list.ActionDelegate = this;
            base.OnStart();
        }

        public void OnClickExec()
        {
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Send":
                    {
                        if (current == null) return;

                        if (current.Exec == "サーバ")
                        {
                            var send = new CheatSend();
                            send.command = current.Command;
                            send.param = Params.Where(p => p.gameObject.activeSelf).Select(p => p.input.text).ToArray();
                            Protocol.Send(send, (r) =>
                            {
                                Entity.Instance.UpdateUserState(r.userState);
                                Entity.Instance.EggList.Modify(r.egg);
                                Entity.Instance.PetList.Modify(r.pet);
                                Entity.Instance.Inventory.Modify(r.items);
                            });
                        }
                        else if(current.Exec == "ローカル")
                        {
                            ExecLocalCommand(current);
                        }
                    }
                    break;
                case "Switch":
                    {
                        Background.SetActive(!Background.activeSelf);
                        if (Background.activeSelf)
                        {
                            list.ReloadData();
                        }
                    }
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }

        /// <summary>
        /// ローカルコマンドの実行
        /// </summary>
        void ExecLocalCommand(Cheat cheat)
        {
            switch (cheat.Command)
            {
                case "DeleteAccessToken":
                    Network.Auth.DeleteAccessToken();
                    break;
            }
        }
    }
}
