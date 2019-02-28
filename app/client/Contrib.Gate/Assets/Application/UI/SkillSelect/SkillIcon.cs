using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entities;
using System.Linq;
using System;

namespace UI
{
    public class SkillIcon : MonoBehaviour
    {
        public Sprite[] Attributes;
        public Image Attribute1;
        public Image Attribute2;

        public Sprite[] SkillTypes;
        public Image SkillType;

        public Entities.InventoryItem item;
        public void Setup(Entities.InventoryItem item, bool selected)
        {
            this.item = item;

            var skill = Entity.Instance.Skills.FirstOrDefault(v => v.Identify == item.identify);

            if (skill != null)
            {
                var attr = skill.Attribute.Where(v => v != 0).OrderBy(v => v);//.ToArray();
                Attribute1.fillAmount = attr.ElementAtOrDefault(0) / 10f;
                Attribute2.fillAmount = attr.ElementAtOrDefault(1) / 10f;
            }
            //if (item.identify == Identify.Empty)
            //{
            //    desc.text = "はずす";
            //}
            //else
            //{
            //    desc.text = $"{Entity.Name(item.identify)}\n{item.num}";
            //}

            //GetComponentInChildren<Image>().color = selected ? Color.red : Color.white;
        }
    }
}
