/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// BinderList
    /// </summary>
    public class BinderListSend
    {

    }

    public class BinderListReceive
    {
		public string[] ids; // 取得済ペットのID一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(BinderListSend obj, Action<BinderListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("BinderList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
