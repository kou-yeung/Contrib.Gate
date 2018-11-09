using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using UnityEngine.Advertisements;
using System.Linq;

namespace UI
{
    public class EggWindow : Window, ANZCellView.IActionDelegate
    {
        public ANZCellView eggCell;
        public ANZCellView hatchCell;
        public GameObject eggItemPrefab;
        public GameObject hatchItemPrefab;


        /// <summary>
        /// タマゴ一覧
        /// </summary>
        class EggList : ANZCellView.IDataSource
        {
            public GameObject eggItemPrefab;
            public EggList(GameObject eggItemPrefab)
            {
                this.eggItemPrefab = eggItemPrefab;
            }
            public GameObject CellViewItem(int index, GameObject item)
            {
                if (item == null) item = Instantiate(eggItemPrefab);
                item.GetComponent<EggItem>().Setup(Entity.Instance.EggList.items[index]);
                return item;
            }

            public Vector2 ItemSize()
            {
                return eggItemPrefab.GetComponent<RectTransform>().sizeDelta;
            }

            public int NumOfItems()
            {
                return Entity.Instance.EggList.items.Count;
            }
        }

        /// <summary>
        /// 孵化リスト
        /// </summary>
        class HatchList : ANZCellView.IDataSource
        {
            public GameObject hatchItemPrefab;
            public HatchList(GameObject hatchItemPrefab)
            {
                this.hatchItemPrefab = hatchItemPrefab;
            }

            public GameObject CellViewItem(int index, GameObject item)
            {
                if (item == null) item = Instantiate(hatchItemPrefab);
                var hatch = Entity.Instance.HatchList.items[index];
                item.GetComponent<HatchItem>().Setup(hatch);
                return item;
            }

            public Vector2 ItemSize()
            {
                return hatchItemPrefab.GetComponent<RectTransform>().sizeDelta;
            }

            public int NumOfItems()
            {
                return Entity.Instance.HatchList.items.Count;
            }
        }

        public void TapCellItem(int index, GameObject listItem)
        {
            Entities.EggItem egg = listItem.GetComponent<EggItem>()?.egg;
            if(egg == null) egg = listItem.GetComponent<HatchItem>()?.egg;

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
                        eggCell.ReloadData();
                        hatchCell.ReloadData();
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
                            eggCell.ReloadData();  // リスト更新
                            hatchCell.ReloadData();

                            Open<PetDetailWindow>(r.item.uniqid);
                        });
                    });
                }
                else
                {
                    DialogWindow.OpenYesNo("確認", $"孵化残り時間： {remain / 60}:{remain % 60}\n広告を観て 30分短縮しますか？", () =>
                    {
                        var window = Window.Open<AdvertisementWindow>(AdReward.Hatch, egg.uniqid);
                        window.OnCloseEvent += eggCell.ReloadData;
                        window.OnCloseEvent += hatchCell.ReloadData;
                    });
                }
            }
            else
            {

                // 同時孵化最大数チェック
                if (Entity.Instance.HatchList.items.Count < (int)Const.MaxHatch)
                {
                    DialogWindow.OpenYesNo("確認", $"{egg.race}のタマゴを孵化予約します", () =>
                    {
                        Protocol.Send(new HatchReserveSend { uniqid = egg.uniqid }, (r) =>
                        {
                            Entity.Instance.HatchList.Modify(r.item);
                            eggCell.ReloadData();  // リスト更新
                            hatchCell.ReloadData();  // リスト更新
                        });
                    });
                }
                else
                {
                    DialogWindow.OpenOk("確認", "これ以上の予約ができません");
                }
            }
        }

        protected override void OnStart()
        {
            // 孵化一覧
            hatchCell.DataSource = new HatchList(hatchItemPrefab);
            hatchCell.ActionDelegate = this;
            hatchCell.ReloadData();

            // タマゴ一覧
            eggCell.DataSource = new EggList(eggItemPrefab);
            eggCell.ActionDelegate = this;
            eggCell.ReloadData();
            base.OnStart();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
