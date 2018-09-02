using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util.Time;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        // KiiCloudの認証
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
                SceneManager.LoadSceneAsync(SceneName.Title);

            }
        });
    }

    public void OnCreate()
    {
    }

    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

