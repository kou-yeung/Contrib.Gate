///=============================
/// 
///=============================
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using KiiCorp.Cloud.Storage;
using System;
using System.Text;
using Util;

namespace Network
{
    /// <summary>
    /// KiiCloudの通信にエラー発生したときのデータ構造
    /// </summary>
    [Serializable]
    class KiiCloudError
    {
        [Serializable]
        public class Details
        {
            public string errorCode;
            public string message;
        }
        public string errorCode;
        public string message;
        public Details details;
    }
    /// <summary>
    /// KiiCloudの通信に成功したときのデータ構造
    /// </summary>
    [Serializable]
    class KiiCloudReturn
    {
        public string returnedValue;
    }

    public class Requests
    {
        public void Send(string method, string json = "{}", Action<bool, string> cb = null)
        {
            CoroutineRunner.Run(InternalSend(method, json, cb));
        }

        IEnumerator InternalSend(string method, string json, Action<bool, string> cb)
        {
            var url = string.Format("https://api-jp.kii.com/api/apps/{0}/server-code/versions/current/{1}", Kii.AppId, method);
            var request = new UnityWebRequest(url, "POST");

            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", KiiUser.AccessToken));
            request.SetRequestHeader("Content-Type", "application/json");

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error))
            {
                var res = JsonUtility.FromJson<KiiCloudReturn>(request.downloadHandler.text);
                if (cb != null) cb(true, res.returnedValue);
            }
            else
            {
                var error = JsonUtility.FromJson<KiiCloudError>(request.downloadHandler.text);
                Debug.Log(JsonUtility.ToJson(error, true));
                if (cb != null) cb(false, request.error);
            }
        }
    }
}
