/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Debug
    /// </summary>
    public class DebugSend
    {

    }

    public class DebugReceive
    {
		public string message;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(DebugSend obj, Action<DebugReceive> cb)
        {
            new Communication("Debug").Push(obj).Send((res, str) => OnReceive(res, str, cb));
        }
    }
}
