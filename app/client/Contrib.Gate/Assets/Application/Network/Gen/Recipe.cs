/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// Recipe
    /// </summary>
    public class RecipeSend
    {
		public uint identify; // 実行したいレシピID
    }

    public class RecipeReceive
    {
		public uint identify; // 獲得されたもの
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(RecipeSend obj, Action<RecipeReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("Recipe").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
