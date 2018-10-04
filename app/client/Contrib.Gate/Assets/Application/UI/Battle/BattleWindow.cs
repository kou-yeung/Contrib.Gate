using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event;
using Network;
using Entities;

namespace UI
{
    public class BattleWindow : Window
    {
        public const string CloseEvent = @"BattleWindow:Close";
        public Button[] Enemies;
        public Button[] Players;
        BattleBeginReceive battleInfo;
        StageInfo stageInfo;

        protected override void OnOpen(params object[] args)
        {
            battleInfo = args[0] as BattleBeginReceive;
            stageInfo = Entity.Instance.StageInfo;

            // 敵配置
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (i < battleInfo.enemies.Length)
                {
                    Enemies[i].gameObject.SetActive(true);
                    var enemy = battleInfo.enemies[i];
                    var image = Enemies[i].GetComponent<Image>();

                }
                else
                {
                    Enemies[i].gameObject.SetActive(false);
                }
            }
            // 自分のユニット配置
            for (int i = 0; i < Players.Length; i++)
            {
                if (!string.IsNullOrEmpty(stageInfo.pets[i]))
                {
                    Players[i].gameObject.SetActive(true);
                    var image = Players[i].GetComponent<Image>();
                    var pet = Entity.Instance.Pets.Find(stageInfo.pets[i]);
                    image.sprite = Resources.LoadAll<Sprite>($"Familiar/{ new Identify(pet.id).Id}/walk")[3];
                }
                else
                {
                    // 配置してない
                    Players[i].gameObject.SetActive(false);
                }
            }

            base.OnOpen(args);
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }

        public void OnDebugEnd()
        {
            Protocol.Send(new BattleEndSend { guid = battleInfo.guid }, battleEnd =>
            {
                Entity.Instance.UserState.AddCoin(battleEnd.coin);
                Entity.Instance.Pets.Modify(battleEnd.exps);

                Protocol.Send(new BattleRewardSend { guid = battleEnd.guid }, battleReward =>
                {
                    Entity.Instance.Inventory.Add(battleReward.items);
                    Entity.Instance.Eggs.Modify(battleReward.eggs);

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
