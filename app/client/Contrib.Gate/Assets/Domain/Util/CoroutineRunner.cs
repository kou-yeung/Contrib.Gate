///========================
/// コルーチンを実行する
///========================
using System.Collections;
using UnityEngine;
using System;

namespace Util
{
    public class CoroutineRunner : MonoBehaviour
    {
        static MonoBehaviour mb;

        static CoroutineRunner()
        {
            var go = new GameObject("CoroutineRunner");
            mb = go.AddComponent<CoroutineRunner>();
            GameObject.DontDestroyOnLoad(go);
        }

        public static Coroutine Run(IEnumerator routine)
        {
            return mb.StartCoroutine(routine);
        }

        public static Coroutine Run(IEnumerator routine, Action cb)
        {
            return mb.StartCoroutine(StartCoroutineWithCallback(routine, cb));
        }

        static IEnumerator StartCoroutineWithCallback(IEnumerator routine, Action cb)
        {
            yield return routine;
            cb?.Invoke();
        }
    }
}
