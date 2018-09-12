//====================
// EntitiesGen から自動生成されます。直接編集しないでください
//====================

// UserCreateStep
enum UserCreateStep {
	EnterName,// // 名前入力
	Prologue,// // プロローグ
	Created,// // 作成済
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
}

// ErrorCode
enum ErrorCode {
	None=0,
	InvalidAdsCode,// // 無効な広告コード
	Network=9999,// // ネットワークエラー
}

