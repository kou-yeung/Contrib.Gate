/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// BattleExp
    /// </summary>
    public class BattleExpSend
    {
		public string guid; // バトルID
    }

    public class BattleExpReceive
    {
		public string guid; // バトルID
		public ExpItem[] exps; // 経験値
		public string debug;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(BattleExpSend obj, Action<BattleExpReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("BattleExp").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
