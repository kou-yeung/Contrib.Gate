using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;

namespace UI
{
    public class EffectWindow : Window
    {
        public Monitor monitor;
        static EffectWindow instance;
        static public EffectWindow Instance
        {
            get
            {
                if (instance == null) instance = Open<EffectWindow>();
                return instance;
            }
        }

        public void Play(string fn, Vector3 screenPos)
        {
            var root = monitor.MonitorRoot;
            var location = monitor.Camera.ScreenToWorldPoint(screenPos);
            EffectManager.Instance.Play(fn, location);
        }
    }
}
