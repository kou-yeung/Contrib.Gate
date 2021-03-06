/// <summary>
/// ProtocolGen から自動生成されます。直接編集しないでください
/// </summary>
using System;
using Entities;

namespace Network
{
    /// <summary>
    /// CreateUser
    /// </summary>
    public class CreateUserSend
    {
		public string name;
    }

    public class CreateUserReceive
    {
		public UserState userState;
    }

    /// <summary>
    /// 送信用メソッド一覧
    /// </summary>
    public static partial class Protocol
    {
        public static void Send(CreateUserSend obj, Action<CreateUserReceive> cb, Func<ErrorCode, bool> error = null)
        {
            new Communication("CreateUser").Push(obj).Send((res, str) => OnReceive(res, str, cb, error));
        }
    }
}
