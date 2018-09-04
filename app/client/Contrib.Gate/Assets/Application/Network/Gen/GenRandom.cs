/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// GenRandom
    /// </summary>
    public class GenRandomSend
    {
		public int num;
    }

    public class GenRandomReceive
    {
		public int[] results;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(GenRandomSend obj, Action<GenRandomReceive> cb)
        {
            new Communication("GenRandom").Push(obj).Send((res, str) => OnReceive(res, str, cb));
        }
    }
}
