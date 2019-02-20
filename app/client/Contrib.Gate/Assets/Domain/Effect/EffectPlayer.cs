using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Effect
{
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeField]
        Image image;
        Sprite[] sprites;

        public bool IsEnded { get; private set; }

        int count;
        int framePreCell;
        public void Play(string name, int framePreCell = 3)
        {
            this.count = 0;
            this.framePreCell = framePreCell;
            sprites = Resources.LoadAll<Sprite>($"Effects/{name}");
            image.sprite = sprites[0];
            image.SetNativeSize();
            IsEnded = false;
        }

        void Update()
        {
            count++;
            var index = count / framePreCell;
            if (index < sprites.Length)
            {
                image.sprite = sprites[index];
            }
            else
            {
                IsEnded = true;
            }
        }
    }
}
