/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// PetDelete
    /// </summary>
    public class PetDeleteSend
    {
		public string uniqid; // ペットUniqid
    }

    public class PetDeleteReceive
    {
		public InventoryItem[] items; // 更新したアイテム一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(PetDeleteSend obj, Action<PetDeleteReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("PetDelete").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
