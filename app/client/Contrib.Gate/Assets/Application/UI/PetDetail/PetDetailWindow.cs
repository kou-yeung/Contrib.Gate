﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using Event;
using Network;

namespace UI
{
    public class PetDetailWindow : Window
    {
        public const string ModifyEvent = @"PetDetailWindow:Modify";
        public const string CloseEvent = @"PetDetailWindow:Close";
        public Text[] param;
        public Image face;
        public new Text name;
        public Text level;
        public Text unit;

        string uniqid;
        UnitList modify;

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;
            if (args.Length >= 2) modify = args[1] as UnitList;
            Setup();
            Observer.Instance.Subscribe(UnitList.UpdateEvent, OnSubscribe);
            base.OnOpen(args);
        }

        void Setup()
        {
            var item = Entity.Instance.PetList.items.Find(v => v.uniqid == uniqid);
            for (int i = 0; i < (int)Param.Count; i++)
            {
                if ((Param)i == Param.Luck) continue;
                param[i].text = item.GetParam((Param)i).ToString();
            }
            name.text = item.Familiar.Name;
            var id = new Entities.Identify(item.id);
            face.sprite = Resources.LoadAll<Sprite>($"Familiar/{id.Id}/face")[0];
            level.text = $"Lv.{item.Level.ToString()}";

            // ユニットにセットしている？
            unit.text = Exists() ? "ユニットから外す" : "ユニットにセットする";
        }

        void OnSubscribe(string name, object o)
        {
            switch (name)
            {
                case UnitList.UpdateEvent:
                    Setup();
                    break;
                case PowerupWindow.CloseEvent:
                    Setup();
                    break;
            }
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Unit":
                    Observer.Instance.Notify(ModifyEvent, uniqid);
                    break;
                case "Powerup":
                    {
                        Open<PowerupWindow>(uniqid);
                        Observer.Instance.Subscribe(PowerupWindow.CloseEvent, OnSubscribe);
                    }
                    break;
                case "Skill":
                    {
                        var send = new SkillLearnSend();
                        send.uniqid = uniqid;
                        send.skill = new Identify(IDType.Skill, 1001);
                        Protocol.Send(send, r =>
                        {
                            Entity.Instance.Inventory.Modify(r.item);
                            Entity.Instance.PetList.Modify(r.pet);
                        });
                    }
                    break;
                default:
                    base.OnButtonClick(btn);
                    break;
            }
        }

        protected override void OnClose()
        {
            Observer.Instance.Unsubscribe(UnitList.UpdateEvent, OnSubscribe);
            Observer.Instance.Notify(CloseEvent);
            base.OnClose();
        }

        bool Exists()
        {
            if (modify != null) return modify.Exists(uniqid);
            else return Entity.Instance.UnitList.Exists(uniqid);
        }
    }
}
