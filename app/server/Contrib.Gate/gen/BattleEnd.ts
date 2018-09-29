//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class BattleEndSend {
    guid: string; // バトルID

	// params -> BattleEndSend
	static Parse(params : any): BattleEndSend {
		return JSON.parse(params.data[0]) as BattleEndSend;
	}
}

class BattleEndReceive {
    coin: number; // お金
    rewards: number[]; // 報酬一覧
    exps: ExpItem[]; // 経験値
	
	// BattleEndReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function BattleEnd(params, context, done) {

    // 受信データをパースする
    let s = BattleEndSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new BattleEndReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/