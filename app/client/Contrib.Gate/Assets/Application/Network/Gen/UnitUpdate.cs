/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// UnitUpdate
    /// </summary>
    public class UnitUpdateSend
    {
		public UnitItem[] items; // Unit情報一覧
    }

    public class UnitUpdateReceive
    {
		public UnitItem[] items; // Unit情報一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(UnitUpdateSend obj, Action<UnitUpdateReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("UnitUpdate").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
