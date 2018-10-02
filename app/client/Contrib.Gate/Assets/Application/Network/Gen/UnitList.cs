/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// UnitList
    /// </summary>
    public class UnitListSend
    {

    }

    public class UnitListReceive
    {
		public UnitItem[] items; // Unit情報一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(UnitListSend obj, Action<UnitListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("UnitList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
