/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Login
    /// </summary>
    public class LoginSend
    {

    }

    public class LoginReceive
    {
		public string appVersion; // // アプリバージョン
		public long timestamp; // // サーバ時間
		public UserState userState;
		public int[] iv;
		public int[] key;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(LoginSend obj, Action<LoginReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Login").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
