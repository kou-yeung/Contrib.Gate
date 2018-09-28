/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// BattleBegin
    /// </summary>
    public class BattleBeginSend
    {

    }

    public class BattleBeginReceive
    {
		public string[] enemies; // 敵一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(BattleBeginSend obj, Action<BattleBeginReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("BattleBegin").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
