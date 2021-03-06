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
		Unknown,// 不明
		Hatch,// 孵化時間短縮
		Unit,// ユニット解放
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
		Enemy,// 敵
		EnemyGroup,// 敵グループ
		Drop,// ドロップ
		Skill,// スキル
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
		StageCantMove,// ステージ移動できません
		StageLoss,// ステージ消失
		HatchMax,// 孵化最大数に達した
		Common=9998,// 一般エラー(一時対応時使用します)
		Network=9999,// ネットワークエラー
	}
	
    /// <summary>
    /// Const
    /// </summary>
	public enum Const
	{
		MaxHatch=3,// 同時最大孵化数
		MaxPetInUnit=3,// ユニットに配置最大ペット数
		MaxUnit=5,// ユニット最大数
		FreeUnit=1,// 無料ユニット数(残りは広告をみて開放する)
	}
	
    /// <summary>
    /// Move
    /// </summary>
	public enum Move
	{
		None,// なし
		Up,// 上の階へ移動
		Down,// 下の階へ移動
	}
	
    /// <summary>
    /// Race
    /// </summary>
	public enum Race
	{
		Beast,
		Undead,
		Fly,
		Insect,
		Plant,
		Amorphas,
		Metal,
		Dragon,
		Human,
		Other,
	}
	
    /// <summary>
    /// Param
    /// </summary>
	public enum Param
	{
		HP,// 体力
		MP,// 魔力
		Attack,// 攻撃
		Defense,// 防御
		Agility,// 素早さ
		Magic,// 魔力
		Count,// 数
	}
	
    /// <summary>
    /// Score
    /// </summary>
	public enum Score
	{
		Physical,// 体
		Power,// 力
		Strength,// 強
		Agility,// 敏
		Magic,// 魔
		Count,// 数
	}
	
    /// <summary>
    /// SkillType
    /// </summary>
	public enum SkillType
	{
		Physical,// 物理
		Magic,// 魔法
		Buff,// バフ
		Debuff,// デバフ
	}
	
    /// <summary>
    /// SkiiTarget
    /// </summary>
	public enum SkiiTarget
	{
		Self,// 自分
		Friend,// 味方
		Rival,// 相手
	}
	
    /// <summary>
    /// Attribute
    /// </summary>
	public enum Attribute
	{
		Earth,// 土
		Water,// 水
		Fire,// 火
		Wind,// 風
		Count,// 数
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
		public uint stageId;// ステージID
		public uint dungeonId;// ダンジョンID
		public int seed;// 乱数シード
		public string guid;// StageBegin時生成される値、フロア移動時のチェックに使用します
		public long lossTime;// 消失時間
		public Move move;// 移動方向
		public string[] pets;// ペット一覧
	}
	
    /// <summary>
    /// EggItem
    /// </summary>
    [Serializable]
	public partial class EggItem
	{
		public string uniqid;// 識別ID
		public bool judgment;// 鑑定済？
		public long createTime;// 生成時間
		public Race race;// 種族(鑑定したらわかる)
		public int rarity;// レアリティ(鑑定したらわかる)
		public uint stage;// 入手ステージID
	}
	
    /// <summary>
    /// PetItem
    /// </summary>
    [Serializable]
	public partial class PetItem
	{
		public string uniqid;// 識別ID
		public uint id;// 使い魔ID
		public long createTime;// 生成時間
		public int level;// レベル
		public int exp;// 経験値
		public int powerupCount;// 餌付け回数(最大回数は レベル - 1)
		public int[] param;// 餌付けによる増加パラメータ(Param)
		public uint skill;// スキル
	}
	
    /// <summary>
    /// HatchItem
    /// </summary>
    [Serializable]
	public partial class HatchItem
	{
		public string uniqid;// 識別ID
		public long startTime;// 開始時間
		public long timeRequired;// 秒数
	}
	
    /// <summary>
    /// EnemyItem
    /// </summary>
    [Serializable]
	public partial class EnemyItem
	{
		public uint id;// 敵のID
		public int level;// レベル
	}
	
    /// <summary>
    /// ExpItem
    /// </summary>
    [Serializable]
	public partial class ExpItem
	{
		public string uniqid;// 識別ID
		public int exp;// 更新後所持経験値
		public int add;// 増加した経験値
		public int level;// レベル
		public bool levelup;// レベルアップ
	}
	
    /// <summary>
    /// UnitItem
    /// </summary>
    [Serializable]
	public partial class UnitItem
	{
		public int id;// Unit番号
		public string[] uniqids;// 識別ID一覧
		public long expirationDate;// 有効期間 [無制限(-1) 未開放(0) それ以外(サーバ時間)]
		public string name;// ユニット名(ユーザのメモ入力領域)
	}
	
    /// <summary>
    /// StageItem
    /// </summary>
    [Serializable]
	public partial class StageItem
	{
		public uint id;// ステージID
		public long clear;// 最後のクリアタイム
	}
	
}
