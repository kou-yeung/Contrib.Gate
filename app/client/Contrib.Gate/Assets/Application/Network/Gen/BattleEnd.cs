/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// BattleEnd
    /// </summary>
    public class BattleEndSend
    {
		public string guid; // バトルID
    }

    public class BattleEndReceive
    {
		public string guid; // 報酬ID
		public int coin; // お金
		public string debug;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(BattleEndSend obj, Action<BattleEndReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("BattleEnd").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
