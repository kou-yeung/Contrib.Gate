using UnityEngine;
using System;
using Entities;

namespace Network
{
    public static partial class Protocol
    {
        public static event Action<ErrorCode> OnError = (error) => { };

        /// <summary>
        /// 受信時の共通処理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="res"></param>
        /// <param name="str"></param>
        /// <param name="cb"></param>
        static void OnReceive<T>(ErrorCode res, string str, Action<T> cb, Func<ErrorCode, bool> error = null)
        {
            if (res == ErrorCode.None)
            {
#if UNITY_EDITOR
                Debug.Log(str);
#endif
                cb(JsonUtility.FromJson<T>(str));
            }
            else
            {
                if (error == null || !error(res))
                {
                    OnError(res);
                }
            }
        }
    }
}

