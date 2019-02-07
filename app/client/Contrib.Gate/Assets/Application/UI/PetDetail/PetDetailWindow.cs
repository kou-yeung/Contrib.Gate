using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using EventSystem;
using Network;
using Util;

namespace UI
{
    public class PetDetailWindow : Window
    {
        //public const string ModifyEvent = @"PetDetailWindow:Modify";
        public const string CloseEvent = @"PetDetailWindow:Close";
        public Text[] param;
        public Image face;
        public new Text name;
        public Text level;
        public Text exp;
        public Text skill;
        public Slider expGauge;
        public Button deleteBtn;
        public Slider[] Attribute;
        string uniqid;

        protected override void OnOpen(params object[] args)
        {
            uniqid = args[0] as string;
            var canDelete = args.Length >= 2 ? (bool)args[1] : true;
            deleteBtn.gameObject.SetActive(canDelete);

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
            face.sprite = Resources.Load<Sprite>($"Familiar/{item.Familiar.Image}/base");
            level.text = $"Lv.{item.level.ToString()}";

            var start = (float)Entity.Instance.LevelTable.Exp(item.level);
            var end = (float)Entity.Instance.LevelTable.Exp(item.level + 1);
            expGauge.value = (item.exp - start) / (end - start);
            exp.text = $"{item.exp}/{end}";

            skill.text = Entity.Name(item.skill);

            // 属性
            EnumExtension<Attribute>.ForEach(v =>
            {
                var index = (int)v;
                this.Attribute[index].value = item.Familiar.Attribute[index];
            });
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
                case PowerupWindow.PowerupEvent:
                    Setup();
                    break;
                case SkillSelectWindow.CloseEvent:
                    Setup();
                    break;
                case SkillSelectWindow.ChangeEvent:
                    Setup();
                    break;
            }
        }

        protected override void OnButtonClick(Button btn)
        {
            switch (btn.name)
            {
                case "Powerup":
                    {
                        Open<PowerupWindow>(uniqid);
                        Observer.Instance.Subscribe(PowerupWindow.CloseEvent, OnSubscribe);
                        Observer.Instance.Subscribe(PowerupWindow.PowerupEvent, OnSubscribe);
                    }
                    break;
                case "Skill":
                    {
                        Debug.Log("スキル選択UIを開く");
                        Open<SkillSelectWindow>(uniqid);
                        Observer.Instance.Subscribe(SkillSelectWindow.CloseEvent, OnSubscribe);
                        Observer.Instance.Subscribe(SkillSelectWindow.ChangeEvent, OnSubscribe);
                    }
                    break;
                case "Delete":
                    {
                        var item = Entity.Instance.PetList.items.Find(v => v.uniqid == uniqid);
                        DialogWindow.OpenYesNo("確認", $"{Entity.Name(item.id)}削除してもよろしいでしょうか", () =>
                        {
                            Protocol.Send(new PetDeleteSend { uniqid = uniqid }, r =>
                            {
                                Entity.Instance.Inventory.Modify(r.items);
                                Entity.Instance.PetList.Remove(item);
                                this.Close();
                            });
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
    }
}
