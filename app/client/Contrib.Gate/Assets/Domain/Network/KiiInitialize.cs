using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public static class KiiInitialize
    {
        static GameObject instance;
        public static void Init()
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate(Resources.Load<GameObject>("Configs/KiiInitialize"));
                GameObject.DontDestroyOnLoad(instance);
            }
        }
    }
}
