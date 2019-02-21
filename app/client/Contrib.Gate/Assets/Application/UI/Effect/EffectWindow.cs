using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;
using System;

namespace UI
{
    public class EffectWindow : Window
    {
        //public Monitor monitor;
        static EffectWindow instance;
        static public EffectWindow Instance
        {
            get
            {
                if (instance == null) instance = Open<EffectWindow>();
                return instance;
            }
        }
        public EffectPlayer Play(string fn, Vector3 screenPos, Action ended = null)
        {
            return Play(fn, screenPos, 1, ended);
        }

        public EffectPlayer Play(string fn, Vector3 screenPos, float scale, Action ended = null)
        {
            if (string.IsNullOrEmpty(fn))
            {
                ended?.Invoke();
                return null;
            }
            else
            {
                var player = EffectManager.Instance.Play(fn, screenPos, ended);
                player.transform.SetParent(this.transform);
                player.transform.localScale = Vector3.one * scale;
                return player;
            }
        }
    }
}
