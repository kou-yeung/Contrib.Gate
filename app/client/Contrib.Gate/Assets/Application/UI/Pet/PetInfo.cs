using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using UnityEngine.UI;
using System;

namespace UI
{
    public class PetInfo : MonoBehaviour
    {
        public Entities.PetItem pet { get; private set; }
        public Text[] param;
        public Text powerup;
        public PrefabLinker face;

        public GameObject unitRoot;
        public GameObject addRoot;

        public void Setup(Entities.PetItem item)
        {
            this.pet = item;

            if (item != null)
            {
                for (int i = 0; i < (int)Param.Count; i++)
                {
                    var value = item.GetParam((Param)i);

                    var format = "{0}";
                    if ((Param)i == Param.HP) format = "HP.{0}";
                    else if ((Param)i == Param.MP) format = "MP.{0}";
                    param[i].text = string.Format(format, value.ToString());
                }

                face.GetComponentInChildren<FaceIcon>().Setup(item);
                //level.text = $"Lv.{item.level}";
                powerup.text = $"{item.powerupCount}/{item.level}";
            }

            unitRoot.SetActive(item != null);
            addRoot.SetActive(item == null);

        }
    }
}
