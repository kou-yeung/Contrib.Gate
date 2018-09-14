using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Event
{
    public class Observer : MonoBehaviour
    {
        static Observer instance;
        static Dictionary<string, Action<string, object>> collection = new Dictionary<string, Action<string, object>>();

        public static Observer Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("Observer");
                    GameObject.DontDestroyOnLoad(go);
                    instance = go.AddComponent<Observer>();
                }
                return instance;
            }
        }

        /// <summary>
        /// 購読する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void Subscribe(string name, Action<string, object> action)
        {
            Action<string, object> cb;
            if (!collection.TryGetValue(name, out cb))
            {
                collection.Add(name, (s, o) => { });
            }
            collection[name] += action;
        }

        /// <summary>
        /// 購読をやめる
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void Unsubscribe(string name, Action<string, object> action)
        {
            Action<string, object> cb;
            if (collection.TryGetValue(name, out cb))
            {
                cb -= action;
            }
        }

        /// <summary>
        /// 通知を送る
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arg"></param>
        public void Notify(string name, object arg = null)
        {
            Action<string, object> cb;
            if (collection.TryGetValue(name, out cb))
            {
                cb(name, arg);
            }
        }
    }
}
