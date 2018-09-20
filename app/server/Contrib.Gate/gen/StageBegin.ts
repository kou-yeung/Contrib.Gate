//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class StageBeginSend {
    stageId: number; // ステージID

	// params -> StageBeginSend
	static Parse(params : any): StageBeginSend {
		return JSON.parse(params.data[0]) as StageBeginSend;
	}
}

class StageBeginReceive {
    stageInfo: StageInfo; // ステージ情報
	
	// StageBeginReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function StageBegin(params, context, done) {

    // 受信データをパースする
    let s = StageBeginSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new StageBeginReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/