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
	Coin,// // コイン
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
	Network=9999,// ネットワークエラー
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
}
