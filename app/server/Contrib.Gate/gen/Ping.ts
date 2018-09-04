//====================
// ProtocolGen から自動生成されます。直接編集しないでください
//====================

class PingSend {
    message: string;

	// params -> PingSend
	static Parse(params : any): PingSend {
		return JSON.parse(params.data[0]) as PingSend;
	}
}

class PingReceive {
    message: string;
    timestamp: number;
	
	// PingReceive -> string
	Pack(): string {
		return JSON.stringify(this);
    }
}

/*
// 以下プロトコルを実装する
// MEMO : 直接にここに実装してしまうと、自動生成時に上書きされてしまいますのでご注意を!!!
function Ping(params, context, done) {

    // 受信データをパースする
    let s = PingSend.Parse(params);

    GetUser(context, (user) => {
		// 返信
	    let r = new PingReceive();

		// TODO : 返信パラメータを設定する

        done(r.Pack());
    });
}
*/