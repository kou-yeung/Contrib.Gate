/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Judgment
    /// </summary>
    public class JudgmentSend
    {
		public string guid; // 未鑑定タマゴID
    }

    public class JudgmentReceive
    {
		public EggItem egg; // タマゴ
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(JudgmentSend obj, Action<JudgmentReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Judgment").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
