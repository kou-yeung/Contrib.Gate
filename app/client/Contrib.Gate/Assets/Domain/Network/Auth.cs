///================================
/// KiiCloudにログイン
/// MEMO : これはゲームにログインという意味ではない。単なるMBaaSを利用するの認証です
///================================
using System;
using UnityEngine;
using KiiCorp.Cloud.Storage;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace Network
{
    public static class Auth
    {
        static readonly string AccessTokenKey = @"AccessTokenKey";
        static string AccessToken
        {
            get
            {
                return PlayerPrefs.GetString(AccessTokenKey, "");
            }
            set
            {
                PlayerPrefs.SetString(AccessTokenKey, value);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// 認証する
        /// ・アクセストークンがあれば、アクセストークンでログインする
        /// ・アクセストークンがなければ、仮ユーザを生成しアクセストークンを発行する
        /// </summary>
        /// <param name="action"></param>
        public static void Authentication(Action<KiiUser> action)
        {
            if (string.IsNullOrEmpty(AccessToken))
            {
                RegisterAsPseudoUser(action);
            }
            else
            {
                LoginWithToken(action);
            }
        }

        /// <summary>
        /// 仮ユーザの作成
        /// </summary>
        /// <param name="action"></param>
        static void RegisterAsPseudoUser(Action<KiiUser> action)
        {
            KiiUser.RegisterAsPseudoUser(new UserFields(), (user, e) =>
            {
                if (e == null)
                {
                    AccessToken = KiiUser.AccessToken;
                    action(user);
                } else
                {
                    action(null);
                } 
            });
        }


        /// <summary>
        /// アクセストークンでログインする
        /// </summary>
        /// <param name="action"></param>
        static void LoginWithToken(Action<KiiUser> action)
        {
            KiiUser.LoginWithToken(AccessToken, (user, e) =>
            {
                if (e == null)
                {
                    AccessToken = KiiUser.AccessToken;
                    action(user);
                }
                else
                {
                    action(null);
                }
            });
        }
#if UNITY_EDITOR
        [MenuItem("Debug/Auth/Delete AccessToken")]
        static void DeleteAccessToken()
        {
            Auth.AccessToken = "";
        }
#endif
    }
}

