using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util.Time;
using System.Linq;
using UnityEngine.UI;

public class Boot : MonoBehaviour
{
    public InputField input;
    public Button btn;

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
                //Protocol.Send(new PingSend { message = "Hello!!" }, (r) =>
                //{
                //    Debug.Log(UnixTime.FromUnixTime(r.timestamp));
                //    Debug.Log(r.message);
                //});

                Protocol.Send(new LoginSend(), (r) =>
                {
                    Debug.Log(r.step);

                    ServerTime.Init(r.timestamp);

                    Debug.Log(UnixTime.FromUnixTime(r.timestamp));
                    switch (r.step)
                    {
                        case Entities.UserCreateStep.EnterName:
                            input.gameObject.SetActive(true);
                            btn.gameObject.SetActive(true);
                            break;
                    }
                });

                //Protocol.Send(new ServerDebugSend(), (r) =>
                //{
                //    Debug.Log(r.param);
                //    Debug.Log(r.context);
                //});
            }
        });
    }

    public void OnCreate()
    {
        Protocol.Send(new CreateUserSend { name = input.text }, (r) =>
        {
            Debug.Log(r.step);
            input.gameObject.SetActive(false);
            btn.gameObject.SetActive(false);
        });
    }

    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

