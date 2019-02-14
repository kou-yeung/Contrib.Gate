using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using System.Text;
using Entities;
using EventSystem;

namespace UI
{
    public class BattleResultWindow : Window
    {
        public const string CloseEvent = @"BattleResultWindow:Close";
        public Text rewardText;

        protected override void OnOpen(params object[] args)
        {
            var sb = new StringBuilder();

            foreach (var arg in args)
            {
                BattleEnd(arg, sb);
                BattleExp(arg, sb);
                BattleReward(arg, sb);
                StageEnd(arg, sb);
            }
            rewardText.text = sb.ToString();
            base.OnOpen(args);
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }

        private void BattleEnd(object receive, StringBuilder sb)
        {
            if (receive == null) return;
            var battleEnd = receive as BattleEndReceive;
            if (battleEnd == null) return;

            sb.AppendLine($"獲得コイン：{battleEnd.coin}");
        }

        private void BattleExp(object receive, StringBuilder sb)
        {
            if (receive == null) return;
            var battleExp = receive as BattleExpReceive;
            if (battleExp == null) return;
            foreach (var item in battleExp.exps)
            {
                var pet = Entity.Instance.PetList.items.Find(v => v.uniqid == item.uniqid);
                if (pet != null)
                {
                    if (item.levelup)
                    {
                        sb.AppendLine($"{Entity.Name(pet.id)} +{item.add} exp [Level Up]");
                    }
                    else
                    {
                        sb.AppendLine($"{Entity.Name(pet.id)} +{item.add} exp");
                    }
                }
            }
        }

        private void BattleReward(object receive, StringBuilder sb)
        {
            if (receive == null) return;
            var battleReward = receive as BattleRewardReceive;
            if (battleReward == null) return;

            foreach (var item in battleReward.items)
            {
                sb.AppendLine($"{Entity.Name(item.identify)} +{item.num}");
            }

            if (battleReward.eggs != null && battleReward.eggs.Length > 0)
            {
                sb.AppendLine($"タマゴ？ x{battleReward.eggs.Length}");
            }
        }

        private void StageEnd(object receive, StringBuilder sb)
        {
            if (receive == null) return;
            var stageEnd = receive as StageEndReceive;
            if (stageEnd == null) return;

            if (stageEnd.eggs != null && stageEnd.eggs.Length > 0)
            {
                sb.AppendLine($"タマゴ？ x{stageEnd.eggs.Length}");
            }
        }
    }
}
