/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Powerup
    /// </summary>
    public class PowerupSend
    {
		public string uniqid; // ペットUniqid
		public InventoryItem[] items; // 使用予定アイテム一覧
    }

    public class PowerupReceive
    {
		public InventoryItem[] items; // 使用したアイテム一覧
		public PetItem pet; // 更新したペット情報
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(PowerupSend obj, Action<PowerupReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Powerup").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
