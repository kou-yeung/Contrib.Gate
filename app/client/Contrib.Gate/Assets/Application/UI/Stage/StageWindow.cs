using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xyz.AnzFactory.UI;
using Entities;
using Network;
using UnityEngine.SceneManagement;
using System.Linq;

namespace UI
{
    public class StageWindow : Window, ANZListView.IDataSource, ANZListView.IActionDelegate
    {
        public ANZListView list;
        public GameObject stageItemPrefab;
        public Text stageRenew;
        public Stage[] stages;  // 表示可能なステージ一覧

        public float HeightItem()
        {
            return stageItemPrefab.GetComponent<RectTransform>().rect.height;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(stageItemPrefab);
            item.GetComponent<StageItem>().Setup(stages[index]);
            return item;
        }

        public int NumOfItems()
        {
            return stages.Length;
        }

        public void TapListItem(int index, GameObject listItem)
        {
            var item = listItem.GetComponent<StageItem>();
            var stage = item.stage;
            /// HACK とりあえずユニットを送る
            /// 将来はユニット選択させる
            var pats = Entity.Instance.UnitList.items[0].uniqids.ToArray();

            Protocol.Send(new StageBeginSend { stageId = stage.Identify, pets = pats }, (r) =>
            {
                // ステージ情報を保存してシーンを変更する
                Entity.Instance.UpdateStageInfo(r.stageInfo);
                SceneManager.LoadScene(SceneName.InGame);
            });
        }

        protected override void OnStart()
        {
            stages = Entity.Instance.Stages.Where(v => v.IsOpen()).ToArray();
            list.DataSource = this;
            list.ActionDelegate = this;
            list.ReloadData();
            base.OnStart();
        }


        void Update()
        {
            if (Entity.Instance.StageList.LocalUpdatePeriod())
            {
                list.ReloadData();
            }
            var period = Util.Time.UnixTime.FromUnixTime(Entity.Instance.StageList.period);
            var now = Util.Time.UnixTime.FromUnixTime(Util.Time.ServerTime.CurrentUnixTime);
            var remain = period - now;
            stageRenew.text = remain.ToString("hh\\:mm\\:ss");
        }
    }
}
