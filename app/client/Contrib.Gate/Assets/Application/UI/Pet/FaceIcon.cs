using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FaceIcon : MonoBehaviour
    {
        public Image face;
        public Text level;

        public void Setup(Entities.PetItem item)
        {
            face.sprite = item.GetFaceImage();
            level.text = $"Lv.{item.level}";
        }
    }
}
