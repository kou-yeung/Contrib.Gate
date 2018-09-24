/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// HatchList
    /// </summary>
    public class HatchListSend
    {

    }

    public class HatchListReceive
    {
		public HatchItem[] items; // 孵化情報一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(HatchListSend obj, Action<HatchListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("HatchList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
