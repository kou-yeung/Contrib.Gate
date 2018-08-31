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

    }

    public class ServerDebugReceive
    {
		public string param;
		public string context;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(ServerDebugSend obj, Action<ServerDebugReceive> cb)
        {
            new Communication("ServerDebug").Push(obj).Send((res, str) => OnReceive(res, str, cb));
        }
    }
}
