/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Vending
    /// </summary>
    public class VendingSend
    {
		public uint identify;
    }

    public class VendingReceive
    {
		public uint identify;
		public uint added;
		public uint current;
		public uint coin;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(VendingSend obj, Action<VendingReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Vending").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
