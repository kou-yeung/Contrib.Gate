/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// FinishPrologue
    /// </summary>
    public class FinishPrologueSend
    {

    }

    public class FinishPrologueReceive
    {
		public UserCreateStep step;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(FinishPrologueSend obj, Action<FinishPrologueReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("FinishPrologue").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
