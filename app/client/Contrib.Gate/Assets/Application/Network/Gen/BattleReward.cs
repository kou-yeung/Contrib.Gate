/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// BattleReward
    /// </summary>
    public class BattleRewardSend
    {
		public string guid; // バトルID
    }

    public class BattleRewardReceive
    {
		public EggItem[] eggs; // タマゴ
		public InventoryItem[] items; // 追加したアイテム
		public string debug;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(BattleRewardSend obj, Action<BattleRewardReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("BattleReward").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
