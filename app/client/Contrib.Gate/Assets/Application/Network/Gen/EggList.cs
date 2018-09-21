/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// EggList
    /// </summary>
    public class EggListSend
    {

    }

    public class EggListReceive
    {
		public EggItem[] items; // 所持タマゴ一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(EggListSend obj, Action<EggListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("EggList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
