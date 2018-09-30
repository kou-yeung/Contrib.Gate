//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class BattleRewardSend {
    guid: string; // バトルID

	// params -> BattleRewardSend
	static Parse(params : any): BattleRewardSend {
		return JSON.parse(params.data[0]) as BattleRewardSend;
	}
}

class BattleRewardReceive {
    eggs: EggItem[]; // タマゴ
    items: InventoryItem[]; // 追加したアイテム
    debug: string;
	
	// BattleRewardReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function BattleReward(params, context, done) {

    // 受信データをパースする
    let s = BattleRewardSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new BattleRewardReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/