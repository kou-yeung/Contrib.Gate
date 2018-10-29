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
        public Image image;

        public void Setup(Entities.PetItem item)
        {
            this.pet = item;
            for (int i = 0; i < (int)Param.Count; i++)
            {
                if (i == (int)Param.Luck) continue;
                var value = item.GetParam((Param)i);
                param[i].text = value.ToString();
            }

            image.sprite = item.GetFaceImage();
        }
    }
}
