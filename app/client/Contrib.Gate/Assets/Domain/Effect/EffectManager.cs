using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;
using System;

namespace Effect
{
    public class EffectManager : MonoBehaviour
    {
        class Handle
        {
            public EffekseerHandle handle;
            public Action ended;
        }

        static EffectManager instance;
        public static EffectManager Instance
        {
            get
            {
                if(instance == null)
                {
                    var go = new GameObject("EffectManager");
                    instance = go.AddComponent<EffectManager>();
                }
                return instance;
            }
        }

        static List<Handle> handles = new List<Handle>();

        /// <summary>
        /// エフェクト名を指定して再生する
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="location"></param>
        /// <param name="ended"></param>
        /// <returns></returns>
        public EffekseerHandle Play(string fn, Vector3 location, Action ended = null)
        {
            var handle = EffekseerSystem.PlayEffect(fn, location);
            handles.Add(new Handle { handle = handle, ended = ended });
            return handle;
        }

        private void Update()
        {
            for (int i = handles.Count - 1; i >= 0; i--)
            {
                if (!handles[i].handle.exists)
                {
                    handles[i].ended?.Invoke();
                    handles.RemoveAt(i);
                }
            }
        }
    }
}
