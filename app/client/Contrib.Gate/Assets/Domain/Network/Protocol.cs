using UnityEngine;
using System;

namespace Network
{
    public static partial class Protocol
    {
        public static event Action<string> OnError = (error) => { };

        /// <summary>
        /// 受信時の共通処理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="res"></param>
        /// <param name="str"></param>
        /// <param name="cb"></param>
        static void OnReceive<T>(bool res, string str, Action<T> cb)
        {
            if (res) cb(JsonUtility.FromJson<T>(str));
            else OnError(str);
        }
    }
}

