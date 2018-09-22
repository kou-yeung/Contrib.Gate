/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Hatch
    /// </summary>
    public class HatchSend
    {
		public string uniqid; // タマゴ Uniqid
    }

    public class HatchReceive
    {
		public PetItem item; // 結果
		public EggItem deleteEgg; // 孵化したタマゴ
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(HatchSend obj, Action<HatchReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Hatch").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
