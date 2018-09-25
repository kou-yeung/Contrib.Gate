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
	PhysicalAttack,// 物理攻撃
	PhysicalDefense,// 物理防御
	MagicAttack,// 魔法攻撃
	MagicDefense,// 魔法防御
	Agility,// 素早さ
	Luck,// 運
	Reserve1,// 予約
	Reserve2,// 予約
	Reserve3,// 予約
	Reserve4,// 予約
	Reserve5,// 予約
	Reserve6,// 予約
	Reserve7,// 予約
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
	dungeonId: number;// ダンジョンID
	seed: number;// 乱数シード
	guid: string;// StageBegin時生成される値、フロア移動時のチェックに使用します
	lossTime: number;// 消失時間
	move: Move;// 移動方向
}

/// <summary>
/// EggItem
/// </summary>
class EggItem {
	uniqid: string;// 識別ID
	race: Race;// 種族
	rarity: number;// レアリティ
	createTime: number;// 生成時間
}

/// <summary>
/// PetItem
/// </summary>
class PetItem {
	uniqid: string;// 識別ID
	id: number;// 使い魔ID
	createTime: number;// 生成時間
	exp: number;// 経験値(レベルはRuntimeで計算します)
	powerupCount: number;// 餌付け回数(最大回数は レベル - 1)
	param: number[];// 増加パラメータ(Param)
}

/// <summary>
/// HatchItem
/// </summary>
class HatchItem {
	uniqid: string;// 識別ID
	startTime: number;// 開始時間
	timeRequired: number;// 秒数
}
