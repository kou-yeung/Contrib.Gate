using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effect;
using System;

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

        public EffekseerHandle Play(string fn, Vector3 screenPos, Action ended = null)
        {
            var root = monitor.MonitorRoot;
            var location = monitor.Camera.ScreenToWorldPoint(screenPos);
            return EffectManager.Instance.Play(fn, location + Vector3.forward * 100, ended);
        }
    }
}
