//====================
// EntitiesGen から自動生成されます。直接編集しないでください
//====================

// UserCreateStep
enum UserCreateStep {
	EnterName,// 名前入力
	Prologue,// プロローグ
	Created,// 作成済
}

// AdReward
enum AdReward {
	Unknown,// 不明
	Hatch,// 孵化時間短縮
	Unit,// ユニット解放
}

// IDType
enum IDType {
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

// ErrorCode
enum ErrorCode {
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

// Const
enum Const {
	MaxHatch=3,// 同時最大孵化数
	MaxPetInUnit=3,// ユニットに配置最大ペット数
	MaxUnit=5,// ユニット最大数
	FreeUnit=1,// 無料ユニット数(残りは広告をみて開放する)
}

// Move
enum Move {
	None,// なし
	Up,// 上の階へ移動
	Down,// 下の階へ移動
}

// Race
enum Race {
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

// Param
enum Param {
	HP,// 体力
	MP,// 魔力
	Attack,// 攻撃
	Defense,// 防御
	Agility,// 素早さ
	Magic,// 魔力
	Count,// 数
}

// Score
enum Score {
	Physical,// 体
	Power,// 力
	Strength,// 強
	Agility,// 敏
	Magic,// 魔
	Count,// 数
}

// SkillType
enum SkillType {
	Physical,// 物理
	Magic,// 魔法
	Buff,// バフ
	Debuff,// デバフ
}

// SkiiTarget
enum SkiiTarget {
	Self,// 自分
	Friend,// 味方
	Rival,// 相手
}

// Attribute
enum Attribute {
	Earth,// 土
	Water,// 水
	Fire,// 火
	Wind,// 風
	Count,// 数
}


/// <summary>
/// InventoryItem
/// </summary>
class InventoryItem {
	identify: number;
	num: number;// 所持数
}

/// <summary>
/// UserState
/// </summary>
class UserState {
	createStep: UserCreateStep;
	playerName: string;
	coin: number;
}

/// <summary>
/// StageInfo
/// </summary>
class StageInfo {
	stageId: number;// ステージID
	dungeonId: number;// ダンジョンID
	seed: number;// 乱数シード
	guid: string;// StageBegin時生成される値、フロア移動時のチェックに使用します
	lossTime: number;// 消失時間
	move: Move;// 移動方向
	pets: string[];// ペット一覧
}

/// <summary>
/// EggItem
/// </summary>
class EggItem {
	uniqid: string;// 識別ID
	judgment: boolean;// 鑑定済？
	createTime: number;// 生成時間
	race: Race;// 種族(鑑定したらわかる)
	rarity: number;// レアリティ(鑑定したらわかる)
	stage: number;// 入手ステージID
}

/// <summary>
/// PetItem
/// </summary>
class PetItem {
	uniqid: string;// 識別ID
	id: number;// 使い魔ID
	createTime: number;// 生成時間
	level: number;// レベル
	exp: number;// 経験値
	powerupCount: number;// 餌付け回数(最大回数は レベル - 1)
	param: number[];// 餌付けによる増加パラメータ(Param)
	skill: number;// スキル
}

/// <summary>
/// HatchItem
/// </summary>
class HatchItem {
	uniqid: string;// 識別ID
	startTime: number;// 開始時間
	timeRequired: number;// 秒数
}

/// <summary>
/// EnemyItem
/// </summary>
class EnemyItem {
	id: number;// 敵のID
	level: number;// レベル
}

/// <summary>
/// ExpItem
/// </summary>
class ExpItem {
	uniqid: string;// 識別ID
	exp: number;// 更新後所持経験値
	add: number;// 増加した経験値
	level: number;// レベル
	levelup: boolean;// レベルアップ
}

/// <summary>
/// UnitItem
/// </summary>
class UnitItem {
	id: number;// Unit番号
	uniqids: string[];// 識別ID一覧
	expirationDate: number;// 有効期間 [無制限(-1) 未開放(0) それ以外(サーバ時間)]
	name: string;// ユニット名(ユーザのメモ入力領域)
}

/// <summary>
/// StageItem
/// </summary>
class StageItem {
	id: number;// ステージID
	clear: number;// 最後のクリアタイム
}
