/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Cheat
    /// </summary>
    public class CheatSend
    {
		public string command;
		public string[] param;
    }

    public class CheatReceive
    {
		public UserState userState;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(CheatSend obj, Action<CheatReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Cheat").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
