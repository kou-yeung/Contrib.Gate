using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using System;
using Util.Time;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using UnityEngine.Advertisements;
using CsvHelper;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;

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
        Protocol.OnError += (code) =>
        {
            Debug.Log(string.Format("Default OnError : {0}", code));
        };

        // https://github.com/unity3d-jp/unityads-help-jp/wiki/Integration-Guide-for-Unity
        // Ads の初期化待ち
        Advertisement.Initialize("2788195");
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            yield return null;
        }

        // KiiCloudの認証
        Auth.Authentication((user) =>
        {
            if (user != null)
            {
                SceneManager.LoadSceneAsync(SceneName.Title);
                //Protocol.Send(new ServerDebugSend { command = "GenGUID" }, (r) =>
                //{
                //    var id = new Entities.Identify(uint.Parse(r.message));
                //    Debug.Log(r.message);
                //    Debug.Log(id.Type);
                //    Debug.Log(id.Id);
                //});
            }
        });

        //// 広告テスト
        //Protocol.Send(new AdsBeginSend { type = Entities.AdReward.Coin }, (r) =>
        //{
        //    Advertisement.Show(new ShowOptions
        //    {
        //        resultCallback = (res) =>
        //        {
        //            Protocol.Send(new AdsEndSend { id = r.id }, (end) =>
        //            {
        //                Debug.Log(end.result);
        //            });
        //        }
        //    });
        //});

        // CSV の 検証
        var familiars = Parse<Entities.Familiar>("Entities/familiar");
        var materials = Parse<Entities.Materials>("Entities/materials");
    }

    T[] Parse<T>(string fn)
    {
        var str = Resources.Load<TextAsset>(fn).text.Trim();
        using (var csv = new CsvReader(new StringReader(str), CsvHelperRegister.configuration))
        {
            return csv.GetRecords<T>().ToArray();
        }

    }

    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

