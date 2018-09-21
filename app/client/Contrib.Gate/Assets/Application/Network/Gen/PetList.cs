/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// PetList
    /// </summary>
    public class PetListSend
    {

    }

    public class PetListReceive
    {
		public PetItem[] items; // 所持ペット一覧
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(PetListSend obj, Action<PetListReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("PetList").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
