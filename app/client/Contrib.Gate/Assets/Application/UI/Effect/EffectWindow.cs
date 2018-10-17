using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;

namespace UI
{
    public class EffectWindow : Window
    {
        public Monitor monitor;
        static public EffectWindow Instance { get; private set; }

        protected override void OnStart()
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
            base.OnStart();
        }

        public void Play()
        {
            var root = monitor.MonitorRoot;
            EffectManager.Instance.Play("Particle01", root.transform.position);
        }
    }
}
