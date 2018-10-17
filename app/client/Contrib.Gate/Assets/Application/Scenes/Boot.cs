using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using Entities;
using UI;

public class Boot : MonoBehaviour
{
    public InputField input;
    public Button btn;

    IEnumerator Start()
    {
        Window.Open<DebugWindow>();

        Entity.Instance.Load();

        KiiInitialize.Init();
        Protocol.OnError += (code) =>
        {
            var str = Entity.Instance.StringTable.Get(code);
            if (!string.IsNullOrEmpty(str))
            {
                Debug.Log(string.Format("Default OnError : {0}", str));
            }
            else
            {
                Debug.Log(string.Format("Default OnError : {0}", code));
            }
        };

#if UNITY_ADS
        // https://github.com/unity3d-jp/unityads-help-jp/wiki/Integration-Guide-for-Unity
        // Ads の初期化待ち
        Advertisement.Initialize("2788195");
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            yield return null;
        }
#else
        yield return null;
#endif

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
    }


    public void Update()
    {
        //Debug.Log(UnixTime.FromUnixTime(ServerTime.CurrentUnixTime));
    }
}

