//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class AdsEndSend {
    id: string;

	// params -> AdsEndSend
	static Parse(params : any): AdsEndSend {
		return JSON.parse(params.data[0]) as AdsEndSend;
	}
}

class AdsEndReceive {
    item: HatchItem; //  // まだ仮パラメータです
	
	// AdsEndReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function AdsEnd(params, context, done) {

    // 受信データをパースする
    let s = AdsEndSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new AdsEndReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/