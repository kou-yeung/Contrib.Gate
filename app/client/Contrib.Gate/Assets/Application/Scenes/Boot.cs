using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util;

public class Boot : MonoBehaviour
{
    public class Foo
    {
        public string name = "FOO!!!";
    }
    void Start()
    {
        KiiInitialize.Init();
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
                Protocol.Send(new PingSend { message = "Hello!!" }, (r) =>
                {
                    Debug.Log(UnixTime.FromUnixTime(r.timestamp));
                    Debug.Log(r.message);
                });

                Protocol.Send(new LoginSend(), (r) =>
                {
                    Debug.Log(string.Format("Login : {0}", r.state));
                });

                Debug.Log("ログイン成功");
            }
            else
            {
                Debug.Log("ログイン失敗");
            }
        });
    }
}

