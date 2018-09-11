/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// ServerDebug
    /// </summary>
    public class ServerDebugSend
    {
		public string command;
    }

    public class ServerDebugReceive
    {
		public string param;
		public string context;
		public string message;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(ServerDebugSend obj, Action<ServerDebugReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("ServerDebug").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
