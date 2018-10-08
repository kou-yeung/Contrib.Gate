//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class StageEndSend {
    stageInfo: StageInfo; // ステージ情報

	// params -> StageEndSend
	static Parse(params : any): StageEndSend {
		return JSON.parse(params.data[0]) as StageEndSend;
	}
}

class StageEndReceive {
    stage: StageItem; // ステージの更新情報
    eggs: EggItem[]; // タマゴ報酬
    message: string;
	
	// StageEndReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function StageEnd(params, context, done) {

    // 受信データをパースする
    let s = StageEndSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new StageEndReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/