/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// SkillLearn
    /// </summary>
    public class SkillLearnSend
    {
		public string uniqid; // ペットUniqid
		public uint skill; // スキルID
    }

    public class SkillLearnReceive
    {
		public InventoryItem item; // 使用したアイテム(スキルID)
		public PetItem pet; // 更新したペット情報
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(SkillLearnSend obj, Action<SkillLearnReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("SkillLearn").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
