//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class BattleExpSend {
    guid: string; // バトルID

	// params -> BattleExpSend
	static Parse(params : any): BattleExpSend {
		return JSON.parse(params.data[0]) as BattleExpSend;
	}
}

class BattleExpReceive {
    guid: string; // バトルID
    exps: ExpItem[]; // 経験値
    debug: string;
	
	// BattleExpReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function BattleExp(params, context, done) {

    // 受信データをパースする
    let s = BattleExpSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new BattleExpReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/