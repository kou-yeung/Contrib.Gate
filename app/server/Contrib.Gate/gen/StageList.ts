//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class StageListSend {


	// params -> StageListSend
	static Parse(params : any): StageListSend {
		return JSON.parse(params.data[0]) as StageListSend;
	}
}

class StageListReceive {
    period: number; // 期間
    items: StageItem[]; // ステージ情報一覧
	
	// StageListReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function StageList(params, context, done) {

    // 受信データをパースする
    let s = StageListSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new StageListReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/