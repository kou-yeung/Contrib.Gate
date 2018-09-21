/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// StageMove
    /// </summary>
    public class StageMoveSend
    {
		public StageInfo stageInfo; // ステージ情報
    }

    public class StageMoveReceive
    {
		public StageInfo stageInfo; // ステージ情報
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(StageMoveSend obj, Action<StageMoveReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("StageMove").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
