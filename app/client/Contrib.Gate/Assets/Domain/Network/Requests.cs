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
    public class Requests
    {
        static readonly string AppID = "";
        static readonly string AppKey = "";
        static readonly Kii.Site Site = Kii.Site.JP;

        public void Send(string method, string data = "", Action<bool, string> cb = null)
        {
            CoroutineRunner.Run(InternalSend(method, data, cb));
        }

        IEnumerator InternalSend(string method, string data, Action<bool, string> cb)
        {
            var url = string.Format("https://api-jp.kii.com/api/apps/{0}/server-code/versions/current/{1}", Kii.AppId, method);
            var request = new UnityWebRequest(url, "POST");

            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", KiiUser.AccessToken));
            request.SetRequestHeader("Content-Type", "application/json");

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (string.IsNullOrEmpty(request.error))
            {
                if (cb != null) cb(true, request.downloadHandler.text);
            }
            else
            {
                if (cb != null) cb(false, request.error);
            }
        }
    }
}
