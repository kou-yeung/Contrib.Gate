using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public float HeightItem()
        {
            return stageItemPrefab.GetComponent<RectTransform>().rect.height;
        }

        public GameObject ListViewItem(int index, GameObject item)
        {
            if (item == null) item = Instantiate(stageItemPrefab);
            item.GetComponent<StageItem>().Setup(Entity.Instance.Stages[index]);
            return item;
        }

        public int NumOfItems()
        {
            /// MEMO : とりあえずすべてステージを表示する
            return Entity.Instance.Stages.Length;
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
            list.DataSource = this;
            list.ActionDelegate = this;
            list.ReloadData();
            base.OnStart();
        }

    }
}
