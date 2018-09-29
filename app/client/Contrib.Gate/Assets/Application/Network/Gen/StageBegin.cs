/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// StageBegin
    /// </summary>
    public class StageBeginSend
    {
		public uint stageId; // ステージID
		public string[] pets; // ステージに行くペットID
    }

    public class StageBeginReceive
    {
		public StageInfo stageInfo; // ステージ情報
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(StageBeginSend obj, Action<StageBeginReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("StageBegin").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
