/// <summary>
/// EntitiesGen から自動生成されます。直接編集しないでください
/// </summary>
using System;

namespace Entities
{
	
    /// <summary>
    /// UserCreateStep
    /// </summary>
	public enum UserCreateStep
	{
		EnterName,// // 名前入力
		Prologue,// // プロローグ
		Created,// // 作成済
	}
	
    /// <summary>
    /// AdReward
    /// </summary>
	public enum AdReward
	{
		Coin,// // コイン
	}
	
	
    /// <summary>
    /// <comment>
    /// </summary>
    [Serializable]
	public partial class Player
	{
		public string name;// <comment>
	}
	
}
