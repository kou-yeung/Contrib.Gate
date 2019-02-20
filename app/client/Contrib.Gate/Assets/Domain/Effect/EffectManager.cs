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
            public EffectPlayer player;
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
                    effectPrefab = Resources.Load<GameObject>("Effects/Player");
                }
                return instance;
            }
        }

        static List<Handle> handles = new List<Handle>();
        static Stack<EffectPlayer> pool = new Stack<EffectPlayer>();
        static GameObject effectPrefab;

        /// <summary>
        /// エフェクト名を指定して再生する
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="location"></param>
        /// <param name="ended"></param>
        /// <returns></returns>
        public EffectPlayer Play(string fn, Vector3 location, Action ended = null)
        {
            var player = GetEffectPlayer();
            //var palyer = EffekseerSystem.PlayEffect(fn, location);
            player.Play(fn);
            player.transform.position = location;
            handles.Add(new Handle { player = player, ended = ended });
            return player;
        }

        private void Update()
        {
            for (int i = handles.Count - 1; i >= 0; i--)
            {
                if (handles[i].player.IsEnded)
                {
                    handles[i].player.gameObject.SetActive(false);
                    handles[i].ended?.Invoke();
                    pool.Push(handles[i].player);
                    handles.RemoveAt(i);
                }
            }
        }

        static EffectPlayer GetEffectPlayer()
        {
            EffectPlayer res = null;
            if (pool.Count > 0)
            {
                res = pool.Pop();
                if (res == null) return GetEffectPlayer();
            }
            else
            {
                var go = Instantiate(effectPrefab);
                res = go.GetComponent<EffectPlayer>();
            }
            res.gameObject.SetActive(true);
            return res;
        }
    }
}
