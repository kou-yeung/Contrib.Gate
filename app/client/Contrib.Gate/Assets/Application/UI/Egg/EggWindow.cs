using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using UnityEngine.Advertisements;

namespace UI
{
    public class EggWindow : Window, ANZCellView.IDataSource, ANZCellView.IActionDelegate
    {
        public ANZCellView cell;
        public GameObject eggItemprefab;

        public GameObject CellViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(eggItemprefab);
            item.GetComponent<EggItem>().Setup(Entity.Instance.EggList.items[index]);
            return item;
        }

        public Vector2 ItemSize()
        {
            return eggItemprefab.GetComponent<RectTransform>().sizeDelta;
        }

        public int NumOfItems()
        {
            return Entity.Instance.EggList.items.Count;
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            var egg = listItem.GetComponent<EggItem>().egg;

            if (egg.judgment)
            {
                // 鑑定
                Hatch(egg);
            } else
            {
                DialogWindow.OpenYesNo("確認", $"タマゴを鑑定します", () =>
                {
                    // 未鑑定
                    Protocol.Send(new JudgmentSend { guid = egg.uniqid }, (JudgmentReceive r) =>
                    {
                        Entity.Instance.EggList.Modify(r.egg);
                        cell.ReloadData();
                    });
                });
            }
        }

        /// <summary>
        /// 孵化関連
        /// </summary>
        /// <param name="egg"></param>
        void Hatch(Entities.EggItem egg)
        {
            var hatch = Entity.Instance.HatchList.items.Find(v => v.uniqid == egg.uniqid);
            if (hatch != null)
            {
                var remain = (hatch.startTime + hatch.timeRequired) - Util.Time.ServerTime.CurrentUnixTime;
                if (remain <= 0)
                {
                    DialogWindow.OpenYesNo("確認", $"{egg.race}のタマゴを孵化します", () =>
                    {
                        Protocol.Send(new HatchSend { uniqid = egg.uniqid }, (HatchReceive r) =>
                        {
                            Entity.Instance.PetList.Modify(r.item);
                            Entity.Instance.EggList.Remove(r.deleteEgg);
                            Entity.Instance.HatchList.Remove(r.deleteEgg.uniqid);
                            cell.ReloadData();  // リスト更新
                        });
                    });
                }
                else
                {
                    DialogWindow.OpenYesNo("確認", $"孵化残り時間： {remain / 60}:{remain % 60}\n広告を観て 30分短縮しますか？", () =>
                    {
                        var window = Window.Open<AdvertisementWindow>(AdReward.Hatch, egg.uniqid);
                        window.OnCloseEvent += cell.ReloadData;
                    });
                }
            }
            else
            {
                DialogWindow.OpenYesNo("確認", $"{egg.race}のタマゴを孵化予約します", () =>
                {
                    Protocol.Send(new HatchReserveSend { uniqid = egg.uniqid }, (r) =>
                    {
                        Entity.Instance.HatchList.Modify(r.item);
                        cell.ReloadData();  // リスト更新
                    });
                });
            }
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
