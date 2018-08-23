using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class Boot : MonoBehaviour
{
    void Start()
    {
        KiiInitialize.Init();
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
                Debug.Log("ログイン成功");
            }
            else
            {
                Debug.Log("ログイン失敗");
            }
        });
    }
}

