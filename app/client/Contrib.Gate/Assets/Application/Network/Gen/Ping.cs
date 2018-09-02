/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Ping
    /// </summary>
    public class PingSend
    {
		public string message;
    }

    public class PingReceive
    {
		public string message;
		public long timestamp;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(PingSend obj, Action<PingReceive> cb)
        {
            new Communication("Ping").Push(obj).Send((res, str) => OnReceive(res, str, cb));
        }
    }
}
