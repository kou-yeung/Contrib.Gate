//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class JudgmentSend {
    guid: string; // 未鑑定タマゴID

	// params -> JudgmentSend
	static Parse(params : any): JudgmentSend {
		return JSON.parse(params.data[0]) as JudgmentSend;
	}
}

class JudgmentReceive {
    egg: EggItem; // タマゴ
	
	// JudgmentReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Judgment(params, context, done) {

    // 受信データをパースする
    let s = JudgmentSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new JudgmentReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/