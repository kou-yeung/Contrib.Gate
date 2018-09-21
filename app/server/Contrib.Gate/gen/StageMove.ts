//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class StageMoveSend {
    stageInfo: StageInfo; // ステージ情報

	// params -> StageMoveSend
	static Parse(params : any): StageMoveSend {
		return JSON.parse(params.data[0]) as StageMoveSend;
	}
}

class StageMoveReceive {
    stageInfo: StageInfo; // ステージ情報
	
	// StageMoveReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function StageMove(params, context, done) {

    // 受信データをパースする
    let s = StageMoveSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new StageMoveReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/