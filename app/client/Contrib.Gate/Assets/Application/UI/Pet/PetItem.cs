using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class PetItem : MonoBehaviour
    {
        public Entities.PetItem pet { get; private set; }
        public PrefabLinker face;
        public GameObject unit;

        public void Setup(Entities.PetItem item, bool unit = false)
        {
            this.pet = item;
            var id = new Entities.Identify(item.id);
            face.GetComponentInChildren<FaceIcon>().Setup(item);
            this.unit.SetActive(unit);
        }
    }
}
