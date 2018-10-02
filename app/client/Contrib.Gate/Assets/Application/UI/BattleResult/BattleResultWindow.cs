﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using System.Text;
using Entities;
using Event;

namespace UI
{
    public class BattleResultWindow : Window
    {
        public const string CloseEvent = @"BattleResultWindow:Close";
        public Text rewardText;

        protected override void OnOpen(params object[] args)
        {
            var battleEnd = args[0] as BattleEndReceive;
            var battleReward = args[1] as BattleRewardReceive;

            var sb = new StringBuilder();
            sb.AppendLine($"獲得コイン：{battleEnd.coin}");
            foreach (var item in battleEnd.exps)
            {
                var pet = Entity.Instance.Pets.items.Find(v => v.uniqid == item.uniqid);
                if (pet != null)
                {
                    sb.AppendLine($"{Entity.Name(pet.id)} +{item.add} exp");
                }
            }

            foreach (var item in battleReward.items)
            {
                sb.AppendLine($"{Entity.Name(item.identify)} +{item.num}");
            }

            if (battleReward.eggs != null && battleReward.eggs.Length > 0)
            {
                sb.AppendLine($"タマゴ？ x{battleReward.eggs.Length}");
            }
            rewardText.text = sb.ToString();
            base.OnOpen(args);
        }

        protected override void OnClose()
        {
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }
    }
}