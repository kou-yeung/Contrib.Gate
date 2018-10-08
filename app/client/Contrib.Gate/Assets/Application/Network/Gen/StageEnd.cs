/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// StageEnd
    /// </summary>
    public class StageEndSend
    {
		public StageInfo stageInfo; // ステージ情報
    }

    public class StageEndReceive
    {
		public StageItem stage; // ステージの更新情報
		public EggItem[] eggs; // タマゴ報酬
		public string message;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(StageEndSend obj, Action<StageEndReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("StageEnd").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
