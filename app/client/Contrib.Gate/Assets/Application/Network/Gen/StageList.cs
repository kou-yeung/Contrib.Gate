/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// StageList
    /// </summary>
    public class StageListSend
    {

    }

    public class StageListReceive
    {
		public StageItem[] items; // ステージ情報一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(StageListSend obj, Action<StageListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("StageList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
