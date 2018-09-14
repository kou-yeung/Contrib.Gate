//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class AdsBeginSend {
    type: AdReward;
    param: number;

	// params -> AdsBeginSend
	static Parse(params : any): AdsBeginSend {
		return JSON.parse(params.data[0]) as AdsBeginSend;
	}
}

class AdsBeginReceive {
    id: string;
	
	// AdsBeginReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function AdsBegin(params, context, done) {

    // 受信データをパースする
    let s = AdsBeginSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new AdsBeginReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/