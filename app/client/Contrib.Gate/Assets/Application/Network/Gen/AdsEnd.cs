/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// AdsEnd
    /// </summary>
    public class AdsEndSend
    {
		public string id;
    }

    public class AdsEndReceive
    {
		public HatchItem item; //  // まだ仮パラメータです
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(AdsEndSend obj, Action<AdsEndReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("AdsEnd").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
