/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// HatchReserve
    /// </summary>
    public class HatchReserveSend
    {
		public string uniqid; // タマゴ Uniqid
    }

    public class HatchReserveReceive
    {
		public HatchItem item; // 孵化情報
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(HatchReserveSend obj, Action<HatchReserveReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("HatchReserve").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
