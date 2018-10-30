using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;

namespace UI
{
    public class PetInfo : MonoBehaviour
    {
        public Entities.PetItem pet;
        public Text[] param;
        public Text level;
        public Text powerup;
        public Image image;

        public void Setup(Entities.PetItem item)
        {
            this.pet = item;
            for (int i = 0; i < (int)Param.Count; i++)
            {
                if (i == (int)Param.Luck) continue;
                var value = item.GetParam((Param)i);

                var format = "{0}";
                if ((Param)i == Param.HP) format = "HP.{0}";
                else if ((Param)i == Param.MP) format = "MP.{0}";
                param[i].text = string.Format(format, value.ToString());
            }
            image.sprite = item.GetFaceImage();
            level.text = $"Lv.{item.level}";
            powerup.text = $"{item.powerupCount}/{item.level}";
        }
    }
}
