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

        public void Play()
        {
            var root = monitor.MonitorRoot;
            EffectManager.Instance.Play("Particle01", root.transform.position);
        }
    }
}
