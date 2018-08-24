using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

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
                new Communication("Ping").Push(new Foo()).Send((c) =>
                {
                    var foo = c.Pop<Foo>();
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

