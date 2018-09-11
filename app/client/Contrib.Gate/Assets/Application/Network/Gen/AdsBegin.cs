/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// AdsBegin
    /// </summary>
    public class AdsBeginSend
    {
		public AdReward type;
    }

    public class AdsBeginReceive
    {
		public string id;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(AdsBeginSend obj, Action<AdsBeginReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("AdsBegin").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
