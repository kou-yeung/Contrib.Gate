/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Inventory
    /// </summary>
    public class InventorySend
    {

    }

    public class InventoryReceive
    {
		public InventoryItem[] items; // 所持アイテム一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(InventorySend obj, Action<InventoryReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Inventory").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
