using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util.Time;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CsvHelper;
using System.IO;
using System.Text;
using UnityEngine.Advertisements;

public class Boot : MonoBehaviour
{
    public InputField input;
    public Button btn;

    public class Foo
    {
        public string name = "FOO!!!";
    }
    IEnumerator Start()
    {
        KiiInitialize.Init();

        // KiiCloudの認証
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
                Protocol.Send(new ServerDebugSend { command = "GenGUID" }, (r) =>
                {
                    Debug.Log(r.message);
                });
            }
        });

        // https://github.com/unity3d-jp/unityads-help-jp/wiki/Integration-Guide-for-Unity
        Advertisement.Initialize("2788195");

        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            yield return null;
        }
        //var option = new ShowOptions
        //{
        //    resultCallback = HandleShowResult
        //};

        Protocol.Send(new AdsBeginSend { type = Entities.AdReward.Coin }, (r) =>
        {
            Advertisement.Show(new ShowOptions
            {
                resultCallback = (res) =>
                {
                    Protocol.Send(new AdsEndSend { id = r.id }, (end) =>
                    {
                        Debug.Log(end.result);
                    });
                }
            });
        });

        //// CSV の 検証
        //var str = Resources.Load<TextAsset>("Entities/familiar").text.Trim();
        //using (var csv = new CsvHelper.CsvReader(new StringReader(str)))
        //{
        //    csv.Configuration.HeaderValidated = null;
        //    csv.Configuration.RegisterClassMap<Entities.FamiliarMap>();
        //    var records = csv.GetRecords<Entities.Familiar>().ToArray();
        //}
    }

    void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed. User rewarded credits.");
                break;
            case ShowResult.Skipped:
                Debug.Log("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.Log("Video failed to show.");
                break;
        }
    }

    public void OnCreate()
    {
    }

    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

