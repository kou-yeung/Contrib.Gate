///========================
/// コルーチンを実行する
///========================
using System.Collections;
using UnityEngine;

namespace Util
{
    public class CoroutineRunner
    {
        static MonoBehaviour mb;

        static CoroutineRunner()
        {
            var go = new GameObject("CoroutineRunner");
            mb = go.AddComponent<MonoBehaviour>();
            GameObject.DontDestroyOnLoad(go);
        }

        public static Coroutine Run(IEnumerator routine)
        {
            return mb.StartCoroutine(routine);
        }
    }
}
