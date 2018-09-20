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
		EnterName,// 名前入力
		Prologue,// プロローグ
		Created,// 作成済
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
		Unknown,// 不明
		Familiar,// 使い魔
		Material,// 素材
		Recipe,// レシピ
		Item,// アイテム
		Vending,// 自販機
		Dungeon,// ダンジョン
		Room,// 部屋設定
		Stage,// ステージ
	}
	
    /// <summary>
    /// ErrorCode
    /// </summary>
	public enum ErrorCode
	{
		None=0,
		InvalidAdsCode,// 無効な広告コード
		CoinLack,// コインが不足
		MaterialLack,// 素材が不足
		RecipeInvalid,// 無効なレシピID
		VendingInvalid,// 無効な自販機ID
		StageInvalid,// 無効なステージID
		Network=9999,// ネットワークエラー
	}
	
	
    /// <summary>
    /// InventoryItem
    /// </summary>
    [Serializable]
	public partial class InventoryItem
	{
		public uint identify;
		public int num;// 所持数
	}
	
    /// <summary>
    /// UserState
    /// </summary>
    [Serializable]
	public partial class UserState
	{
		public UserCreateStep createStep;
		public string playerName;
		public int coin;
	}
	
    /// <summary>
    /// StageInfo
    /// </summary>
    [Serializable]
	public partial class StageInfo
	{
		public uint dungeonId;// ダンジョンID
		public uint seed;// 乱数シード
		public string guid;// StageBegin時生成される値、フロア移動時のチェックに使用します
		public long lossTime;// 消失時間
	}
	
}
