//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class GenRandomSend {
    num: number;

	// params -> GenRandomSend
	static Parse(params : any): GenRandomSend {
		return JSON.parse(params.data[0]) as GenRandomSend;
	}
}

class GenRandomReceive {
    results: number[];
	
	// GenRandomReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function GenRandom(params, context, done) {

    // 受信データをパースする
    let s = GenRandomSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new GenRandomReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/