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
    /// IDType
    /// </summary>
	public enum IDType
	{
		Unknown,// //不明
		Familiar,// //使い魔
	}
	
    /// <summary>
    /// ErrorCode
    /// </summary>
	public enum ErrorCode
	{
		None=0,
		InvalidAdsCode,// // 無効な広告コード
		Network=9999,// // ネットワークエラー
	}
	
	
}
