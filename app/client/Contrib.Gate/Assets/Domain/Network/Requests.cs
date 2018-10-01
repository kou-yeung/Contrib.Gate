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
using Entities;

namespace Network
{
    /// <summary>
    /// KiiCloudの通信にエラー発生したときのデータ構造
    /// </summary>
    [Serializable]
    class KiiCloudError
    {
        public const string STEP_COUNT_EXCEEDED = @"STEP_COUNT_EXCEEDED";

        [Serializable]
        public class Details
        {
            public string errorCode = null;
            public string message = null;
        }
        public string errorCode = null;
        public string message = null;
        public Details details = null;
    }
    /// <summary>
    /// KiiCloudの通信に成功したときのデータ構造
    /// </summary>
    [Serializable]
    class KiiCloudReturn
    {
        public string returnedValue = null;
    }

    /// <summary>
    /// ApiError
    /// </summary>
    [Serializable]
    public class ApiError
    {
        public ErrorCode errorCode; // エラーコードが 0 以外だとエラー
        public string message;      // 追加メッセージ
    }

    public class Requests
    {
        public void Send(string method, string json = "{}", Action<ErrorCode, string> cb = null)
        {
            CoroutineRunner.Run(InternalSend(method, json, cb));
        }

        IEnumerator InternalSend(string method, string json, Action<ErrorCode, string> cb, int retry = 2)
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

                if (cb != null)
                {
                    var error = JsonUtility.FromJson<ApiError>(res.returnedValue);
                    if(!string.IsNullOrEmpty(error.message))
                    {
                        Debug.Log(error.message);
                    }
                    cb(error.errorCode, res.returnedValue);
                }
            }
            else
            {
                var error = JsonUtility.FromJson<KiiCloudError>(request.downloadHandler.text);
                Debug.Log(JsonUtility.ToJson(error, true));
                if (retry > 0 && error.errorCode != KiiCloudError.STEP_COUNT_EXCEEDED)
                {
                    yield return InternalSend(method, json, cb, retry - 1);
                }
                else
                {
                    if (cb != null) cb(ErrorCode.Network, request.error);
                }
            }
        }
    }
}
