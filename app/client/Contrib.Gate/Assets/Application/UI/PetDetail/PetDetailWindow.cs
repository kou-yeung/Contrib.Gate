using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;

namespace UI
{
    public class PetDetailWindow : Window
    {
        public Text[] param;
        public Image face;
        public new Text name;
        public Text level;

        Entities.PetItem item;

        protected override void OnOpen(params object[] args)
        {
            item = Entity.Instance.Pets.items.Find(v => v.uniqid == (string)args[0]);
            for (int i = 0; i < (int)Param.Count; i++)
            {
                if ((Param)i == Param.Luck) continue;
                param[i].text = item.GetParam((Param)i).ToString();
            }
            name.text = item.Familiar.Name;
            var id = new Entities.Identify(item.id);
            face.sprite = Resources.LoadAll<Sprite>($"Familiar/{id.Id}/face")[0];
            level.text = $"Lv.{item.Level.ToString()}";
            base.OnOpen(args);
        }
    }
}
