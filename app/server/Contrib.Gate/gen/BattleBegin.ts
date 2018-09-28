//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class BattleBeginSend {
    guid: string; // ステージのguid

	// params -> BattleBeginSend
	static Parse(params : any): BattleBeginSend {
		return JSON.parse(params.data[0]) as BattleBeginSend;
	}
}

class BattleBeginReceive {
    guid: string; // id
    name: string; // グループ名
    enemies: EnemyItem[]; // 敵一覧
	
	// BattleBeginReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function BattleBegin(params, context, done) {

    // 受信データをパースする
    let s = BattleBeginSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new BattleBeginReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/