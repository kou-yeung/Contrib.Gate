using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Network;
using Entities;
using System.Linq;

namespace UI
{
    public class BattleWindow : Window
    {
        public const string CloseEvent = @"BattleWindow:Close";
        public GameObject unitPrefab;
        public GameObject enemiesArea;  // 敵配置エリア
        public GameObject playersArea;  // プレイヤー配置エリア
        //public Button[] Enemies;
        //public Button[] Players;
        //public Image[] cursors;

        BattleBeginReceive battleInfo;
        StageInfo stageInfo;

        protected override void OnOpen(params object[] args)
        {
            battleInfo = args[0] as BattleBeginReceive;
            stageInfo = Entity.Instance.StageInfo;

            // 敵配置
            for (int i = 0; i < battleInfo.enemies.Length; i++)
            {
                var enemy = Instantiate(unitPrefab, enemiesArea.transform);
                var unit = enemy.GetComponent<Unit>();
                unit.Setup(battleInfo.enemies[i]);
            }
            // 自分のユニット配置
            for (int i = 0; i < stageInfo.pets.Length; i++)
            {
                var item = Entity.Instance.Pets.Find(stageInfo.pets[i]);
                if (item != null)
                {
                    var pet = Instantiate(unitPrefab, playersArea.transform);
                    var unit = pet.GetComponent<Unit>();
                    unit.Setup(item);
                }
            }

            base.OnOpen(args);
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }

        protected override void OnButtonClick(Button btn)
        {
            switch(btn.name)
            {
                case "DebugEnd":
                    BattleEnd();
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }
        public void BattleEnd()
        {
            Protocol.Send(new BattleEndSend { guid = battleInfo.guid }, battleEnd =>
            {
                // コインと経験値入手
                Entity.Instance.UserState.AddCoin(battleEnd.coin);
                Entity.Instance.Pets.Modify(battleEnd.exps);

                Protocol.Send(new BattleRewardSend { guid = battleEnd.guid }, battleReward =>
                {
                    // アイテムとタマゴ入手
                    Entity.Instance.Inventory.Add(battleReward.items);
                    Entity.Instance.Eggs.Modify(battleReward.eggs);

                    // 結果を表示する
                    Open<BattleResultWindow>(battleEnd, battleReward);
                    Observer.Instance.Subscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                });
            });
        }

        void OnSubscribe(string name, object o)
        {
            switch (name)
            {
                case BattleResultWindow.CloseEvent:
                    Observer.Instance.Unsubscribe(BattleResultWindow.CloseEvent, OnSubscribe);
                    this.Close();
                    break;
            }
        }
    }
}
